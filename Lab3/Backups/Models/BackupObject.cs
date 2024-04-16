using Backups.Exceptions;
using Backups.Services;

namespace Backups.Models;

public class BackupObject
{
    public BackupObject(string fullPath)
    {
        if (string.IsNullOrWhiteSpace(fullPath))
            throw new BackupsException("Path is empty, so BackupObject can't be created.");

        FullPath = fullPath;
    }

    public string FullPath { get; }
    public string Name => Path.GetFileName(FullPath);
}