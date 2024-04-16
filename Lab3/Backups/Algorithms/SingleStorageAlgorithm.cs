using System.Globalization;
using System.IO.Compression;
using Backups.Exceptions;
using Backups.Models;
using Backups.Services;

namespace Backups.Algorithms;

public class SingleStorageAlgorithm : IStorageAlgorithm
{
    public IReadOnlyCollection<Storage> CreateStorage(BackupTask backupTask, string path, int id)
    {
        if (id < 0)
            throw new BackupsException("Id is null, so Archivating can't be created.");

        if (!backupTask.Repository.IsDirectory(path))
        {
            throw new BackupsException("Path is not a dir, so Single archivation can't be made.");
        }

        if (backupTask is null)
            throw new BackupsException("BT is null, so algorythm can't work.");

        string fullPath =
            backupTask.Repository.CreateZipArchiveForMany(backupTask.BackupObjectsList, path, $"SingleArchive_{id}.zip");

        var storage = new Storage(fullPath, backupTask.Algorithm);

        foreach (BackupObject backupObject in backupTask.BackupObjectsList)
        {
            storage.AddBackupObject(backupObject);
        }

        var storagesList = new List<Storage>();
        storagesList.Add(storage);

        return storagesList;
    }
}