using Backups.Models;
using Zio;
using Zio.FileSystems;

namespace Backups.Services;

public interface IRepository
{
    string FullPath { get; }

    bool IsDirectory(string path);

    bool IsFile(string path);

    bool Exists(string path);

    string CreateDirictory(string pathToDir, string name);

    string CreateZipArchiveForOne(BackupObject backupObject, string path, string name);

    string CreateZipArchiveForMany(IReadOnlyCollection<BackupObject> backupObjectsList, string path, string name);
}