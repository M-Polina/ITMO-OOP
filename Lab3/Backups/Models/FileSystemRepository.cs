using System.IO.Compression;
using System.Text;
using Aspose.Zip;
using Backups.Exceptions;
using Backups.Services;
using Newtonsoft.Json;

namespace Backups.Models;

public class FileSystemRepository : IRepository
{
    private const int _minNumOfObjectsInList = 0;

    [JsonConstructor]
    public FileSystemRepository(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BackupsException("FileSystemRepository path can't be null or empty");

        if (!IsDirectory(name))
            throw new BackupsException("path is not a dirictory.");

        FullPath = name;
    }

    [JsonProperty("name")]
    public string FullPath { get; }

    public bool IsDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            return true;
        }

        return false;
    }

    public bool IsFile(string path)
    {
        if (File.Exists(path))
        {
            return true;
        }

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

        string fullPath = Path.Combine(pathToDir, name);
        Directory.CreateDirectory(fullPath);

        return fullPath;
    }

    public Stream CreateFile(string pathToFile, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BackupsException("Name is null or empty, dir can't be crated.");

        if (IsDirectory(pathToFile))
            throw new BackupsException("pathToDir is not a dir, so it can't be created");

        return File.Open(Path.Combine(pathToFile, name), FileMode.OpenOrCreate);
    }

    public Stream OpenFileToWrite(string path)
    {
        if (IsFile(path))
            throw new BackupsException("pathToDir is not a dir, so it can't be created");

        return File.Open(path, FileMode.Open);
    }

    public IDisposable OpenArchiveToRead(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new BackupsException("Path is null, so Archive can't be opened.");

        if (Path.GetExtension(path).Equals(".zip"))
            throw new BackupsException("Path is not an archive");

        return new Archive(path);
    }

    public string CreateZipArchiveForOne(BackupObject backupObject, string path, string name)
    {
        if (backupObject is null)
            throw new BackupsException("No BachupObjects (it is null) to archivate");

        if (!IsDirectory(path))
            throw new BackupsException("Path is not a dir, archive can't be created.");

        if (string.IsNullOrWhiteSpace(name))
            throw new BackupsException("Name is empty, so archove can't be created.");

        string fullPath = Path.Combine(path, name);
        string startPath = backupObject.FullPath;
        string zipPath = Path.Combine(path, name);

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        if (IsDirectory(startPath))
        {
            using (FileStream zipFile = File.Open(zipPath, FileMode.Create))
            {
                using (var archive = new Archive())
                {
                    DirectoryInfo dir = new DirectoryInfo(startPath);
                    archive.CreateEntries(dir);
                    archive.Save(zipFile);
                }
            }
        }
        else if (IsFile(startPath))
        {
            using (FileStream zipFile = File.Open(zipPath, FileMode.Create))
            {
                using (var archive = new Archive())
                {
                    archive.CreateEntry(Path.GetFileName(startPath), startPath);
                    archive.Save(zipFile);
                }
            }
        }

        return fullPath;
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

        using (FileStream zipFile = File.Open(zipPath, FileMode.Create))
        {
            using (var archive = new Archive())
            {
                foreach (BackupObject backupObject in backupObjectsList)
                {
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    startPath = backupObject.FullPath;

                    if (IsDirectory(startPath))
                    {
                        DirectoryInfo dir = new DirectoryInfo(startPath);
                        archive.CreateEntries(dir);
                    }
                    else if (IsFile(startPath))
                    {
                        archive.CreateEntry(Path.GetFileName(startPath), startPath);
                    }
                }

                archive.Save(zipFile);
            }
        }

        return zipPath;
    }
}