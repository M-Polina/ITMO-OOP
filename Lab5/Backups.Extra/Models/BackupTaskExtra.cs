using Backups.Exceptions;
using Backups.Extra.Algorithms;
using Backups.Extra.Exceptions;
using Backups.Extra.Logger;
using Backups.Extra.Unarchiver;
using Backups.Models;
using Backups.Services;
using Newtonsoft.Json;

namespace Backups.Extra.Models;

public class BackupTaskExtra
{
    private const int MinNumberOfElements = 0;

    [JsonConstructor]
    public BackupTaskExtra(
        string path,
        string name,
        IRepositoryExtension repository,
        IStorageAlgorithm algorithm,
        ISelectRestorePoint restorePointSelector,
        IRestorePointCleaner restorePointCleaner,
        ILogger logger)
    {
        if (restorePointSelector is null)
            throw new BackupsException("restorePointSelector is null, BackupTaskExtra can't be created.");
        if (restorePointCleaner is null)
            throw new BackupsException("restorePointCleaner is null, BackupTaskExtra can't be created.");
        if (logger is null)
            throw new BackupsException("Logger is null, BackupTaskExtra can't be created.");

        Logger = logger;
        RestorePointCleaner = restorePointCleaner;
        RestorePointSelector = restorePointSelector;
        Repository = repository;
        BackupTask = new BackupTask(path, name, repository, algorithm);

        Logger.Log("BackupTask was created.");
    }

    public ILogger Logger { get; }
    public ISelectRestorePoint RestorePointSelector { get; }
    public IRestorePointCleaner RestorePointCleaner { get; }
    public BackupTask BackupTask { get; }
    public IRepositoryExtension Repository { get; }
    [JsonIgnore]
    public IReadOnlyCollection<BackupObject> BackupObjectsList => BackupTask.BackupObjectsList;
    public Backup Backup => BackupTask.Backup;
    public IStorageAlgorithm Algorithm => BackupTask.Algorithm;
    public string Name => BackupTask.Name;
    [JsonProperty("path")]
    public string FullPath => BackupTask.FullPath;
    [JsonIgnore]
    public IReadOnlyCollection<RestorePoint> RestorePointsList => Backup.RestorePointsList;

    public void AddBackupObject(BackupObject backupObject)
    {
        BackupTask.AddBackupObject(backupObject);
        Logger.Log($"BackupObject (Path = {backupObject.FullPath}) was added to BackupTask.");
    }

    public void RemoveBackupObject(BackupObject backupObject)
    {
        BackupTask.RemoveBackupObject(backupObject);
        Logger.Log($"BackupObject (Path = {backupObject.FullPath}) was removed from BackupTask.");
    }

    public void CreateRestorePoint()
    {
        BackupTask.CreateRestorePoint();
        Logger.Log($"RestorePoint was created in BackupTask.");

        List<RestorePoint> pointsToCleanUp = RestorePointSelector.SelectRestorePoints(RestorePointsList).ToList();

        if (pointsToCleanUp.Count == RestorePointsList.Count)
            pointsToCleanUp.Remove(pointsToCleanUp.Last());

        if (pointsToCleanUp.Count != MinNumberOfElements)
            RestorePointCleaner.Clean(pointsToCleanUp, this, Logger);
    }
}