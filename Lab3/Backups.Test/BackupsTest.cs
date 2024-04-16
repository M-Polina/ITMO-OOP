using Backups.Algorithms;
using Backups.Exceptions;
using Backups.Models;
using Xunit;
using Zio;
using Zio.FileSystems;

namespace Backups.Test;

public class BackupsTets
{
    [Fact]
    public void CreateTask_SplitStorageInMemoryRepository_CheckNumberOfRestorePointsAndStorages()
    {
        string repoPath = @"InMemRepo";
        string taskPath = Path.Combine(@"InMemRepo", "BackupTasks");
        string boPath1 = Path.Combine(@"InMemRepo", "Dir1");
        string boPath2 = Path.Combine(@"InMemRepo", "File0.txt");

        var repository = new InMemoryRepository(repoPath);
        repository.CreateDirictory(repoPath, "BackupTasks");

        repository.CreateDirictory(repoPath, "Dir1");
        repository.CreateDirictory(Path.Combine(repoPath, "Dir1"), "Dir2");
        repository.CreateFile(Path.Combine(repoPath, "Dir1"), "File1.txt");
        repository.CreateFile(repoPath, "File0.txt");

        var algorithm = new SplitStorageAlgorithm();

        var backupTask = new BackupTask(taskPath, "Backup 1", repository, algorithm);

        var backupObject1 = new BackupObject(boPath1);
        var backupObject2 = new BackupObject(boPath2);

        backupTask.AddBackupObject(backupObject1);
        backupTask.AddBackupObject(backupObject2);
        backupTask.CreateRestorePoint();

        backupTask.RemoveBackupObject(backupObject1);

        backupTask.CreateRestorePoint();

        Assert.Equal(2, backupTask.Backup.RestorePointsList.Count);
        Assert.Equal(3, backupTask.Backup.AllStoragesList.Count);
    }

    [Fact(Skip = "Local Repository test.")]
    public void CreateTask_SingleStorageLocalRepository_CheckNumberOfRestorePointsAndStorages()
    {
        string repoPath = @"D:\ITMO\2 year\3 semester\OOP\M-Polina\Lab3\Backups";
        string taskPath = @"D:\ITMO\2 year\3 semester\OOP\M-Polina\Lab3\Backups\BackupTasks";
        string boPath1 = @"D:\ITMO\2 year\3 semester\OOP\M-Polina\Lab3\Backups\Utils\Dir11";
        string boPath2 = @"D:\ITMO\2 year\3 semester\OOP\M-Polina\Lab3\Backups\Utils\File1.txt";

        var repository = new FileSystemRepository(repoPath);
        var algorithm = new SplitStorageAlgorithm();

        var backupTask = new BackupTask(taskPath, "Backup 1", repository, algorithm);

        var backupObject1 = new BackupObject(boPath1);
        var backupObject2 = new BackupObject(boPath2);

        backupTask.AddBackupObject(backupObject1);
        backupTask.AddBackupObject(backupObject2);

        backupTask.CreateRestorePoint();
    }
}