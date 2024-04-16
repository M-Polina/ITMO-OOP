using Backups.Exceptions;
using Backups.Services;
using Newtonsoft.Json;

namespace Backups.Models;

public class BackupTask
{
    private const int _minId = 0;
    private int _id = _minId;

    [JsonProperty("backupObjectsListPrivate")]
    private List<BackupObject> _backupObjectsList;

    [JsonConstructor]
    public BackupTask(string path, string name, IRepository repository, IStorageAlgorithm algorithm)
    {
        if (repository is null)
            throw new BackupsException("Repository is null, BackupTask can't be created.");
        if (algorithm is null)
            throw new BackupsException("Algorithm is null, BackupTask can't be created.");
        if (string.IsNullOrWhiteSpace(name))
            throw new BackupsException("Name is null or empty, BT can't be created.");
        if (string.IsNullOrWhiteSpace(path))
            throw new BackupsException("Path is null or empty, BT can't be created.");

        Algorithm = algorithm;
        Repository = repository;
        _backupObjectsList = new List<BackupObject>();
        Name = name;
        FullPath = Path.Combine(path, name);
        Backup = new Backup(FullPath);
        Repository.CreateDirictory(path, name);
    }

    [JsonIgnore]
    public IReadOnlyCollection<BackupObject> BackupObjectsList => _backupObjectsList;
    public Backup Backup { get; }
    public IStorageAlgorithm Algorithm { get; }
    public IRepository Repository { get; }
    public string Name { get; }
    [JsonProperty("path")]
    public string FullPath { get; }

    public void AddBackupObject(BackupObject backupObject)
    {
        if (backupObject is null)
            throw new BackupsException("BackupObject is null so it can't be added to Task.");

        _backupObjectsList.Add(backupObject);
    }

    public void RemoveBackupObject(BackupObject backupObject)
    {
        if (backupObject is null)
            throw new BackupsException("BackupObject is null so it can't be removed from Task.");

        if (_backupObjectsList.SingleOrDefault(obj => obj.FullPath.Equals(backupObject.FullPath)) is null)
            throw new BackupsException("BackupObject doesn't exist, so it can't be removed.");

        _backupObjectsList.Remove(backupObject);
    }

    public void CreateRestorePoint()
    {
        DateTime time = DateTime.Now;
        string dirNameRP = "RestorePoint_" + _id;
        string pathRP = Path.Combine(FullPath, dirNameRP);
        Repository.CreateDirictory(FullPath, dirNameRP);
        IReadOnlyCollection<Storage> storages = Algorithm.CreateStorage(this, pathRP, _id);
        var restorePoint = new RestorePoint(FullPath, storages, _id);

        Backup.AddRestorePoint(restorePoint);
        _id++;
    }

    public void RemoveRestorePoint(RestorePoint restorePoint)
    {
        Backup.RemoveRestorePoint(restorePoint);
    }

    public void AddRestorePoint(RestorePoint restorePoint)
    {
        Backup.AddRestorePoint(restorePoint);
    }
}