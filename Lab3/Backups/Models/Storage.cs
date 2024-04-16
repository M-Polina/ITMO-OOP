using Backups.Exceptions;
using Backups.Services;
using Newtonsoft.Json;

namespace Backups.Models;

public class Storage
{
    [JsonProperty("backupObjectsPrivate")]
    private List<BackupObject> _backupObjectsList = new List<BackupObject>();

    [JsonConstructor]
    public Storage(string path, IStorageAlgorithm algorithm)
    {
        if (algorithm is null)
            throw new BackupsException("Algorithm is null, Storage can't be created.");
        if (string.IsNullOrWhiteSpace(path))
            throw new BackupsException("Path is null or empty, Storage can't be created.");

        Path = path;
        Algorithm = algorithm;
    }

    public IReadOnlyCollection<BackupObject> BackupObjectsList => _backupObjectsList;
    public IStorageAlgorithm Algorithm { get; }
    [JsonProperty("path")]
    public string Path { get; }

    public void AddBackupObject(BackupObject backupObject)
    {
        if (backupObject is null)
            throw new BackupsException("BackupObject object is null, so it can't be added to Storage");

        if (!(_backupObjectsList.SingleOrDefault(bo => bo.FullPath.Equals(backupObject.FullPath)) is null))
            throw new BackupsException("This BO already exists in Storage.");

        _backupObjectsList.Add(backupObject);
    }
}