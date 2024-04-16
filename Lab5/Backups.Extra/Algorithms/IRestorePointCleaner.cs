using Backups.Extra.Logger;
using Backups.Extra.Models;
using Backups.Models;

namespace Backups.Extra.Algorithms;

public interface IRestorePointCleaner
{
    public void Clean(IReadOnlyCollection<RestorePoint> restorePointsList, BackupTaskExtra backupTaskExtra, ILogger logger);
}