using System.Globalization;
using Backups.Exceptions;
using Backups.Models;
using Backups.Services;

namespace Backups.Algorithms;

public class SplitStorageAlgorithm : IStorageAlgorithm
{
    public IReadOnlyCollection<Storage> CreateStorage(BackupTask backupTask, string path, int id)
    {
        if (id < 0)
            throw new BackupsException("Id is null, so Archivating can't be created.");

        if (!backupTask.Repository.IsDirectory(path))
            throw new BackupsException("Path is not a dir, so Split archivation can't be made.");

        if (backupTask is null)
            throw new BackupsException("BT is null, so algorythm can't work.");

        string fullPath;

        var storagesList = new List<Storage>();

        foreach (BackupObject backupObject in backupTask.BackupObjectsList)
        {
            string name = backupObject.Name;
            fullPath = backupTask.Repository.CreateZipArchiveForOne(backupObject, path, name + $"_{id}.zip");
            var storage = new Storage(fullPath, backupTask.Algorithm);
            storage.AddBackupObject(backupObject);
            storagesList.Add(storage);
        }

        return storagesList;
    }
}