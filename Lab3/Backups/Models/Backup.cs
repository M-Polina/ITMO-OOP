using Backups.Exceptions;
using Newtonsoft.Json;

namespace Backups.Models;

public class Backup
{
    [JsonProperty("restorePointsListPrivate")]
    private List<RestorePoint> _restorePointsList = new List<RestorePoint>();
    [JsonProperty("allStoragesListPrivate")]
    private List<Storage> _allStoragesList = new List<Storage>();

    [JsonConstructor]
    public Backup(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new BackupsException("Path is null or empty, Backup can't be created.");

        FullPath = path;
    }

    [JsonProperty("path")]
    public string FullPath { get; }

    [JsonIgnore]
    public IReadOnlyCollection<RestorePoint> RestorePointsList => _restorePointsList;
    [JsonIgnore]
    public IReadOnlyCollection<Storage> AllStoragesList => _allStoragesList;

    public bool ContainsStorage(Storage storage) => !(_allStoragesList.SingleOrDefault(st => st.Path.Equals(storage.Path)) is null);

    public void AddRestorePoint(RestorePoint restorePoint)
    {
        if (restorePoint is null)
            throw new BackupsException("RP is null, so it can't be added to Backup.");

        foreach (var storage in restorePoint.StoragesList)
        {
                _allStoragesList.Add(storage);
        }

        _restorePointsList.Add(restorePoint);
    }

    public void RemoveRestorePoint(RestorePoint restorePoint)
    {
        if (restorePoint is null)
            throw new BackupsException("RP is null, so it can't be added to Backup.");

        _restorePointsList.Remove(restorePoint);

        foreach (var storage in restorePoint.StoragesList)
        {
            _allStoragesList.Remove(storage);
        }
    }
}