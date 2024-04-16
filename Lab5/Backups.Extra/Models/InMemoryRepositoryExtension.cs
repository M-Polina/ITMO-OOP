using Backups.Extra.Exceptions;
using Backups.Models;
using Backups.Services;
using Newtonsoft.Json;

namespace Backups.Extra.Unarchiver;

public class InMemoryRepositoryExtension : IRepositoryExtension
{
    private const int MinIndDif = 1;
    private const int FirstInd = 0;
    private char separator = System.IO.Path.DirectorySeparatorChar;

    public InMemoryRepositoryExtension(string name)
    {
        InMemoryRepository = new InMemoryRepository(name);
    }

    public InMemoryRepository InMemoryRepository { get; }

    [JsonProperty("path")]
    public string FullPath => InMemoryRepository.FullPath;

    public bool IsDirectory(string path) => InMemoryRepository.IsDirectory(path);
    public bool IsFile(string path) => InMemoryRepository.IsFile(path);
    public bool Exists(string path) => InMemoryRepository.Exists(path);
    public string CreateDirictory(string pathToDir, string name) => InMemoryRepository.CreateDirictory(pathToDir, name);

    public string CreateZipArchiveForOne(BackupObject backupObject, string path, string name) =>
        InMemoryRepository.CreateZipArchiveForOne(backupObject, path, name);

    public string CreateZipArchiveForMany(IReadOnlyCollection<BackupObject> backupObjectsList, string path, string name)
        => InMemoryRepository.CreateZipArchiveForMany(backupObjectsList, path, name);

    public void UnarchiveToDirictory(string archivePath, string dirToSave)
    {
        if (string.IsNullOrWhiteSpace(archivePath) || string.IsNullOrWhiteSpace(dirToSave))
            throw new BackupsExtraExceptions("Incorrect paths while unarciving.");

        RepoObject toCopyObj = InMemoryRepository.GetRepoObject(archivePath);
        RepoObject whereToCopyObj = InMemoryRepository.GetRepoObject(dirToSave);

        if (!(whereToCopyObj is RepoDirectory))
            throw new BackupsExtraExceptions("Not a dir to copy there unarchived file.");

        RepoDirectory whereToCopyDir = (RepoDirectory)whereToCopyObj;

        InMemoryRepository.CreateCopyDir(toCopyObj, whereToCopyDir);
    }

    public string GetStringDirectoryOfFileOrDir(string path)
    {
        return GetDirectoryOfFileOrDir(path).FullPath;
    }

    public RepoDirectory GetDirectoryOfFileOrDir(string path)
    {
        string? dir = Path.GetDirectoryName(path);
        if (string.IsNullOrWhiteSpace(dir))
            throw new BackupsExtraExceptions("Null directory while getting path dir.");
        if (!IsDirectory(dir))
            throw new BackupsExtraExceptions("Not a directory while getting path dir.");

        return (RepoDirectory)InMemoryRepository.GetRepoObject(dir);
    }

    public void WriteToFile(string filePath, string stringToWrite) { }

    public void DeliteFile(string filePath)
    {
        if (!IsFile(filePath))
            throw new BackupsExtraExceptions("Incorrect filePath to delete.");

        RepoFile repoFile = InMemoryRepository.GetRepoFile(filePath);
        RepoDirectory repoDir = GetDirectoryOfFileOrDir(filePath);
        repoDir.RemoveRepoObject(repoFile);
    }

    public void DeliteDirectoryRecursively(string dirPath)
    {
        if (!IsDirectory(dirPath))
            throw new BackupsExtraExceptions("Incorrect filePath to delete.");

        RepoDirectory thisDir = (RepoDirectory)InMemoryRepository.GetRepoObject(dirPath);
        RepoDirectory repoDir = GetDirectoryOfFileOrDir(dirPath);
        repoDir.RemoveRepoObject(thisDir);
    }

    public void CopyObject(string pathToCopy, string newDirOfObjectPath, string newObjectPath)
    {
        RepoObject toCopyObj = InMemoryRepository.GetRepoObject(pathToCopy);
        RepoObject whereToCopyObj = InMemoryRepository.GetRepoObject(newDirOfObjectPath);

        if (!IsDirectory(whereToCopyObj.FullPath))
            throw new BackupsExtraExceptions("Not a dir to copy to while copying.");

        RepoDirectory whereToCopyDir = (RepoDirectory)whereToCopyObj;

        RepoObject? foundObject =
            whereToCopyDir.Children().SingleOrDefault(obj => obj.FullPath.Equals(toCopyObj.FullPath));
        if (foundObject is not null)
        {
            DeleteExisting(foundObject.FullPath);
        }

        if (InMemoryRepository.IsDirectory(toCopyObj.FullPath))
            InMemoryRepository.CreateCopyDir(toCopyObj, whereToCopyDir);

        if (InMemoryRepository.IsFile(toCopyObj.FullPath))
            InMemoryRepository.CreateCopyFile((RepoFile)toCopyObj, whereToCopyDir);
    }

    public void CopyArchiveFromPath(string archivePathToCopy, string newArchivePath)
    {
        RepoObject toCopyObj = InMemoryRepository.GetRepoObject(archivePathToCopy);
        string? path = Path.GetDirectoryName(newArchivePath);

        if (path is null)
            throw new BackupsExtraExceptions("Null directory of n archive dorectory to copy archive to.");

        RepoObject whereToCopyObjDir = InMemoryRepository.GetRepoObject(path);
        RepoDirectory whereToCopyDir = new RepoDirectory(newArchivePath);
        whereToCopyObjDir.AddRepoObject(whereToCopyDir);

        InMemoryRepository.CreateCopyDir(toCopyObj, whereToCopyDir);
    }

    public void DeleteExisting(string path)
    {
        if (InMemoryRepository.IsDirectory(path))
            DeliteDirectoryRecursively(path);

        if (InMemoryRepository.IsFile(path))
            DeliteFile(path);
    }

    public string CreateFile(string pathToDir, string name)
    {
        return InMemoryRepository.CreateFile(pathToDir, name);
    }
}