using Backups.Exceptions;
using Backups.Models;
using Newtonsoft.Json;

namespace Backups.Models;

public class RestorePoint
{
    private const int _minNumOfSroragesInList = 0;
    private const int _minId = 0;

    [JsonProperty("storagesListPrivate")]
    private List<Storage> _storagesList;

    [JsonConstructor]
    public RestorePoint(string path, IReadOnlyCollection<Storage> storagesList, int id)
    {
        if (id < _minId)
            throw new BackupsException("Id is null, so RP can't be created.");
        if (string.IsNullOrWhiteSpace(path))
            throw new BackupsException("Path is null, so RP can't be created.");
        if (storagesList.Count == _minNumOfSroragesInList)
            throw new BackupsException("There is no backup object to create RP");

        CreationTime = DateTime.Now;
        Id = id;
        Name = "RestorePoint_" + id;
        FullPath = Path.Combine(path, Name);
        _storagesList = new List<Storage>();
        _storagesList.AddRange(storagesList);
    }

    public IReadOnlyCollection<Storage> StoragesList => _storagesList;
    public DateTime CreationTime { get; }
    public string Name { get; }

    [JsonProperty("path")]
    public string FullPath { get; }
    public int Id { get; }

    public IReadOnlyCollection<Storage> GetStoragesList() => _storagesList;

    public IReadOnlyCollection<BackupObject> GetBackupObjectsList()
    {
        List<BackupObject> backupObjectsList = new List<BackupObject>();
        foreach (Storage st in _storagesList)
        {
            backupObjectsList.AddRange(st.BackupObjectsList);
        }

        return backupObjectsList;
    }

    public void AddStorage(Storage storage)
    {
        if (storage is null)
            throw new BackupsException("Storage is null, so it can't be add to restore point.");

        if (!(_storagesList.SingleOrDefault(st => st.Path.Equals(storage.Path)) is null))
            throw new BackupsException("Storage is  in RP, so it can't be added to restore point.");

        _storagesList.Add(storage);
    }

    public void RemoveStorages(Storage storage)
    {
        if (storage is null)
            throw new BackupsException("Storage is null, so it can't be deleted from restore point.");

        if (_storagesList.SingleOrDefault(st => st.Path.Equals(storage.Path)) is null)
            throw new BackupsException("Storage is not in RP, so it can't be deleted from restore point.");

        _storagesList.Remove(storage);
    }
}