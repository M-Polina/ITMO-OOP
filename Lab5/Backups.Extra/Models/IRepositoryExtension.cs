using Backups.Models;
using Backups.Services;

namespace Backups.Extra.Unarchiver;

public interface IRepositoryExtension : IRepository
{
    void UnarchiveToDirictory(string archivePath, string dirToSave);
    void DeliteFile(string filePath);
    void DeliteDirectoryRecursively(string dirPath);
    void CopyArchiveFromPath(string archivePathToCopy, string newArchivePath);
    void WriteToFile(string filePath, string stringToWrite);
    void DeleteExisting(string path);
    string GetStringDirectoryOfFileOrDir(string path);
    void CopyObject(string pathToCopy, string newDirOfObjectPath, string newObjectPath);
}