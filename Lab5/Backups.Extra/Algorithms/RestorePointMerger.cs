using Backups.Algorithms;
using Backups.Extra.Exceptions;
using Backups.Extra.Logger;
using Backups.Extra.Models;
using Backups.Models;

namespace Backups.Extra.Algorithms;

public class RestorePointMerger : IRestorePointCleaner
{
    private const int MinNumberOfElements = 0;
    private const int MinDifBetweenId = 1;

    public void Clean(IReadOnlyCollection<RestorePoint> restorePointsList, BackupTaskExtra backupTaskExtra, ILogger logger)
    {
        if (restorePointsList.Count == MinNumberOfElements)
            throw new BackupsExtraExceptions("Nopoints to merge.");
        if (backupTaskExtra is null)
            throw new BackupsExtraExceptions("backupTask is null while deleting it.");
        if (logger is null)
            throw new BackupsExtraExceptions("Logger is null while deleting it.");

        BackupTask backupTask = backupTaskExtra.BackupTask;
        int pointsNumber = restorePointsList.Count;
        int lastPointId;
        if (backupTask.Algorithm is SingleStorageAlgorithm)
        {
            lastPointId = MinNumberOfElements;
            for (int i = 0; i < pointsNumber; i++)
            {
                backupTask.RemoveRestorePoint(restorePointsList.ElementAt(i));
                lastPointId = restorePointsList.ElementAt(i).Id;
            }

            string pathRP = restorePointsList.ElementAt(pointsNumber - MinDifBetweenId).FullPath;
            var storages = restorePointsList.ElementAt(pointsNumber - MinDifBetweenId).StoragesList;
            var restorePoint = new RestorePoint(backupTask.FullPath, storages, lastPointId);
            backupTask.Backup.AddRestorePoint(restorePoint);

            for (int i = 0; i < pointsNumber - MinDifBetweenId; i++)
            {
                backupTaskExtra.Repository.DeliteDirectoryRecursively(restorePointsList.ElementAt(i).FullPath);
            }

            logger.Log("Unneded Restore Points were Merged to new one.");

            return;
        }

        int pointId = MinNumberOfElements;
        List<Storage> storagesList = new List<Storage>();
        foreach (var point in restorePointsList)
        {
            pointId = point.Id;
            foreach (var storage in point.StoragesList)
            {
                Storage? foundStorage = storagesList.SingleOrDefault(st =>
                    st.BackupObjectsList.ElementAt(MinNumberOfElements).FullPath.Equals(storage.BackupObjectsList.ElementAt(MinNumberOfElements).FullPath));
                if (foundStorage is null)
                {
                    storagesList.Add(storage);
                }
                else
                {
                    storagesList.Remove(foundStorage);
                    storagesList.Add(storage);
                }
            }

            backupTask.RemoveRestorePoint(point);
        }

        RestorePoint lastPoint = restorePointsList.Last();
        string newStoragePath;
        foreach (var st in storagesList)
        {
            Storage? foundStorage = lastPoint.StoragesList.SingleOrDefault(stor =>
                stor.BackupObjectsList.ElementAt(MinNumberOfElements).FullPath.Equals(st.BackupObjectsList.ElementAt(MinNumberOfElements).FullPath));

            if (foundStorage == null)
            {
                newStoragePath = Path.Combine(lastPoint.FullPath, $"SingleArchive_{lastPoint.Id}.zip");
                backupTaskExtra.Repository.CopyArchiveFromPath(st.Path, newStoragePath);
                Storage newSt = new Storage(newStoragePath, st.Algorithm);
                lastPoint.AddStorage(newSt);
                newSt.AddBackupObject(st.BackupObjectsList.ElementAt(MinNumberOfElements));
            }
        }

        backupTask.Backup.AddRestorePoint(lastPoint);

        for (int i = 0; i < pointsNumber - MinDifBetweenId; i++)
        {
            backupTaskExtra.Repository.DeliteDirectoryRecursively(restorePointsList.ElementAt(i).FullPath);
        }

        logger.Log("Unneeded Restore Points were Merged to new one.");
    }
}