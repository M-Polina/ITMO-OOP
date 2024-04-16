using Backups.Extra.Exceptions;
using Backups.Extra.Logger;
using Backups.Extra.Models;
using Backups.Models;

namespace Backups.Extra.Algorithms;

public class RestorePointDeleter : IRestorePointCleaner
{
    public void Clean(IReadOnlyCollection<RestorePoint> restorePointsList, BackupTaskExtra backupTaskExtra, ILogger logger)
    {
        if (backupTaskExtra is null)
            throw new BackupsExtraExceptions("backupTaskExtra is null while deleting it.");
        if (logger is null)
            throw new BackupsExtraExceptions("Logger is null while deleting it.");

        foreach (var point in restorePointsList)
        {
            backupTaskExtra.BackupTask.RemoveRestorePoint(point);
            backupTaskExtra.Repository.DeliteDirectoryRecursively(point.FullPath);
        }

        logger.Log("Unneeded Restore Points were deleted.");
    }
}