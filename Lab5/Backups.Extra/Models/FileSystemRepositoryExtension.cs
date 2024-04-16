using System.IO.Compression;
using Backups.Exceptions;
using Backups.Extra.Exceptions;
using Backups.Models;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;

namespace Backups.Extra.Unarchiver;

public class FileSystemRepositoryExtension : IRepositoryExtension
{
    public FileSystemRepositoryExtension(string name)
    {
        FileSystemRepository = new FileSystemRepository(name);
    }

    public FileSystemRepository FileSystemRepository { get; }

    [JsonProperty("name")]
    public string FullPath => FileSystemRepository.FullPath;
    public bool IsDirectory(string path) => FileSystemRepository.IsDirectory(path);
    public bool IsFile(string path) => FileSystemRepository.IsFile(path);
    public bool Exists(string path) => FileSystemRepository.Exists(path);

    public string CreateDirictory(string pathToDir, string name) =>
        FileSystemRepository.CreateDirictory(pathToDir, name);

    public string CreateZipArchiveForOne(BackupObject backupObject, string path, string name) =>
        FileSystemRepository.CreateZipArchiveForOne(backupObject, path, name);

    public string CreateZipArchiveForMany(IReadOnlyCollection<BackupObject> backupObjectsList, string path, string name)
        => FileSystemRepository.CreateZipArchiveForMany(backupObjectsList, path, name);

    public void UnarchiveToDirictory(string archivePath, string dirToSave)
    {
        if (string.IsNullOrWhiteSpace(archivePath) || string.IsNullOrWhiteSpace(dirToSave))
            throw new BackupsExtraExceptions("Incorrect paths while unarciving.");

        ZipFile.ExtractToDirectory(archivePath, dirToSave, true);
    }

    public string GetStringDirectoryOfFileOrDir(string path)
    {
        string? dir = Path.GetDirectoryName(path);
        if (string.IsNullOrWhiteSpace(dir))
            throw new BackupsExtraExceptions("Null dirictory of path.");

        return dir;
    }

    public void DeliteFile(string filePath)
    {
        if (!IsFile(filePath))
            throw new BackupsExtraExceptions("Incorrect filePath to delete.");

        File.Delete(filePath);
    }

    public void DeliteDirectoryRecursively(string dirPath)
    {
        if (!IsDirectory(dirPath))
            throw new BackupsExtraExceptions("Incorrect dirPath to delete.");

        Directory.Delete(dirPath, true);
    }

    public void DeleteExisting(string path)
    {
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }

        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public void CopyObject(string pathToCopy, string newDirOfObjectPath, string newObjectPath)
    {
        if (!Directory.Exists(newDirOfObjectPath))
            throw new BackupsExtraExceptions("Not a dir to copy to while copying object.");

        if (Directory.Exists(pathToCopy))
        {
            if (!Directory.Exists(newObjectPath))
                Directory.CreateDirectory(newObjectPath);

            FileSystem.CopyDirectory(pathToCopy, newObjectPath, true);
        }

        if (File.Exists(pathToCopy))
        {
            File.Copy(pathToCopy, newObjectPath, true);
        }
    }

    public void CopyArchiveFromPath(string archivePathToCopy, string newArchivePath)
    {
        File.Copy(archivePathToCopy, newArchivePath, true);
    }

    public void WriteToFile(string filePath, string stringToWrite)
    {
        if (!IsFile(filePath))
            throw new BackupsExtraExceptions("Incorrect filePath to write to.");

        using (StreamWriter sw = File.AppendText(filePath))
        {
            sw.WriteLine(stringToWrite);
        }
    }
}