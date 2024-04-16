using System.IO.Compression;
using System.Text;
using Aspose.Zip;
using Backups.Exceptions;
using Backups.Services;
using Newtonsoft.Json;

namespace Backups.Models;

public class InMemoryRepository : IRepository
{
    public const int _minNumOfObjectsInList = 0;
    private const int _firstInd = 1;
    private readonly string _repositoryPath;
    private RepoDirectory rootDir;
    private char separator = Path.DirectorySeparatorChar;

    [JsonConstructor]
    public InMemoryRepository(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BackupsException("Name of repository can't be null");

        _repositoryPath = name;
        rootDir = new RepoDirectory(_repositoryPath);
    }

    public string FullPath => _repositoryPath;
    public RepoDirectory RootDir => rootDir;

    public RepoFile GetRepoFile(string path)
    {
        string[] roPaths = path.Split(separator);
        if (!roPaths[0].Equals(rootDir.Name))
            throw new BackupsException("Incorrect path, repoObject can't be gotten.");

        RepoObject repoObj = rootDir;
        for (int ind = _firstInd; ind < roPaths.Length; ind++)
        {
            RepoObject? repoObjectCh = repoObj.Children().SingleOrDefault(ch => ch.Name == roPaths[ind]);

            if (repoObjectCh is null)
            {
                throw new BackupsException("Incorrect path, repoObject can't be gotten.");
            }

            repoObj = repoObjectCh;
        }

        if (!(repoObj is RepoFile))
            throw new BackupsException("Not a file.");

        return (RepoFile)repoObj;
    }

    public RepoObject GetRepoObject(string path)
    {
        string[] roPaths = path.Split(separator);
        if (!roPaths[0].Equals(rootDir.Name))
            throw new BackupsException("Incorrect path, repoObject can't be gotten.");

        RepoObject repoObj = rootDir;
        for (int ind = _firstInd; ind < roPaths.Length; ind++)
        {
            RepoObject? repoObjectCh = repoObj.Children().SingleOrDefault(ch => ch.Name == roPaths[ind]);

            if (repoObjectCh is null)
            {
                throw new BackupsException("Incorrect path, repoObject can't be gotten.");
            }

            repoObj = repoObjectCh;
        }

        return repoObj;
    }

    public RepoObject? FindRepoObject(string path)
    {
        string[] roPaths = path.Split(separator);
        if (!roPaths[0].Equals(rootDir.Name))
            return null;

        RepoObject repoObj = rootDir;
        for (int ind = _firstInd; ind < roPaths.Length; ind++)
        {
            RepoObject? repoObjectCh = repoObj.Children().SingleOrDefault(ch => ch.Name == roPaths[ind]);

            if (repoObjectCh is null)
            {
                return null;
            }

            repoObj = repoObjectCh;
        }

        return repoObj;
    }

    public bool IsDirectory(string path)
    {
        RepoObject? repoObject = FindRepoObject(path);
        if (repoObject is null)
            return false;
        if (repoObject is RepoDirectory)
            return true;
        return false;
    }

    public bool IsFile(string path)
    {
        RepoObject? repoObject = FindRepoObject(path);
        if (repoObject is null)
            return false;
        if (repoObject is RepoFile)
            return true;
        return false;
    }

    public bool Exists(string path)
    {
        return IsFile(path) || IsDirectory(path);
    }

    public string CreateDirictory(string pathToDir, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BackupsException("Name is null or empty, dir can't be crated.");
        }

        if (!IsDirectory(pathToDir))
        {
            throw new BackupsException("pathToDir is not a dir, so it can't be created");
        }

        if (!IsDirectory(pathToDir))
            throw new BackupsException("pathToDir is not a directory.");

        RepoObject repoObject = GetRepoObject(pathToDir);
        string fullPath = Path.Combine(pathToDir, name);
        RepoObject newRepoObject = new RepoDirectory(fullPath);
        repoObject.AddRepoObject(newRepoObject);

        return fullPath;
    }

    public string CreateFile(string pathToDir, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BackupsException("Name is null or empty, dir can't be crated.");
        }

        if (!IsDirectory(pathToDir))
        {
            throw new BackupsException("pathToDir is not a dir, so it can't be created");
        }

        if (!IsDirectory(pathToDir))
            throw new BackupsException("pathToDir is not a directory.");

        RepoObject repoObject = GetRepoObject(pathToDir);
        string fullPath = Path.Combine(pathToDir, name);
        RepoObject newRepoObject = new RepoFile(fullPath);
        repoObject.AddRepoObject(newRepoObject);

        return fullPath;
    }

    public RepoFile CreateCopyFile(RepoFile fileToCopy, RepoDirectory destenationDir)
    {
        if (fileToCopy is null || destenationDir is null)
        {
            throw new BackupsException("fileToCopy or destenationDir is null. File can't be copied.");
        }

        RepoFile file = new RepoFile(Path.Combine(destenationDir.FullPath, fileToCopy.Name));
        return file;
    }

    public void CreateCopyDir(RepoObject objToCopy, RepoDirectory destenationDir)
    {
        if (objToCopy is null || destenationDir is null)
        {
            throw new BackupsException("fileToCopy or destenationDir is null. File can't be copied.");
        }

        RepoObject parentDirctory = (RepoDirectory)GetRepoObject(destenationDir.FullPath);
        RepoDirectory dir = new RepoDirectory(Path.Combine(destenationDir.FullPath, objToCopy.Name));
        parentDirctory.AddRepoObject(dir);
        foreach (RepoObject obj in objToCopy.Children())
        {
            if (obj is RepoFile)
            {
                dir.AddRepoObject(CreateCopyFile(GetRepoFile(obj.FullPath), destenationDir));
            }
            else
            {
                CreateCopyDir(obj, dir);
            }
        }
    }

    public string CreateZipArchiveForOne(BackupObject backupObject, string path, string name)
    {
        if (backupObject is null)
            throw new BackupsException("No BachupObjects (it is null) to archivate");

        if (!IsDirectory(path))
            throw new BackupsException("Path is not a dir, archive can't be created.");

        if (string.IsNullOrWhiteSpace(name))
            throw new BackupsException("Name is empty, so archove can't be created.");

        string startPath = backupObject.FullPath;

        RepoObject objToCopy = GetRepoObject(startPath);
        string zipPath = Path.Combine(path, name);
        RepoDirectory destenationDir = new RepoDirectory(zipPath);
        RepoDirectory dirToCreateZip = (RepoDirectory)GetRepoObject(path);
        dirToCreateZip.AddRepoObject(destenationDir);

        if (IsDirectory(startPath))
        {
            CreateCopyDir(objToCopy, destenationDir);
        }
        else if (IsFile(startPath))
        {
            destenationDir.AddRepoObject(CreateCopyFile(GetRepoFile(objToCopy.FullPath), destenationDir));
        }

        return zipPath;
    }

    public string CreateZipArchiveForMany(IReadOnlyCollection<BackupObject> backupObjectsList, string path, string name)
    {
        if (backupObjectsList.Count == _minNumOfObjectsInList)
            throw new BackupsException("No BachupObjects to archivate");

        if (!IsDirectory(path))
            throw new BackupsException("Path is not a dir, archive can't be created.");

        if (string.IsNullOrWhiteSpace(name))
            throw new BackupsException("Name is empty, so archove can't be created.");

        string startPath;
        string zipPath = Path.Combine(path, name);

        foreach (BackupObject backupObject in backupObjectsList)
        {
            startPath = backupObject.FullPath;
            RepoObject objToCopy = GetRepoObject(startPath);
            RepoDirectory destenationDir = new RepoDirectory(zipPath);
            RepoDirectory dirToCreateZip = (RepoDirectory)GetRepoObject(GetRepoObject(path).FullPath);

            dirToCreateZip.AddRepoObject(destenationDir);

            if (IsDirectory(startPath))
            {
                CreateCopyDir(objToCopy, destenationDir);
            }
            else if (IsFile(startPath))
            {
                CreateCopyFile(GetRepoFile(objToCopy.FullPath), destenationDir);
            }
        }

        return zipPath;
    }
}