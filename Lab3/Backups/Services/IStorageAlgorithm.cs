using Backups.Models;

namespace Backups.Services;

public interface IStorageAlgorithm
{
    IReadOnlyCollection<Storage> CreateStorage(BackupTask backupTask, string path, int id);
}