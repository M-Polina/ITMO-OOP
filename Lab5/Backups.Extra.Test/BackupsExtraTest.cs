using Backups.Algorithms;
using Backups.Extra.Algorithms;
using Backups.Extra.Logger;
using Backups.Extra.Models;
using Backups.Extra.Unarchiver;
using Backups.Models;
using Xunit;

namespace Backups.Extra.Test;

public class BackupsExtraTest
{
    [Fact]
    public void CreateTask_InMemoryRepositoryExtension_RemoveRestorePoint_CheckRestorePointsNumber()
    {
        string repoPath = @"InMemRepo";
        string logfile = Path.Combine(@"InMemRepo", "logfile.log");
        string taskPath = Path.Combine(@"InMemRepo", "BackupTasks");
        string boPath1 = Path.Combine(@"InMemRepo", "Dir1");
        string boPath2 = Path.Combine(@"InMemRepo", "File0.txt");

        var repository = new InMemoryRepositoryExtension(repoPath);
        repository.CreateDirictory(repoPath, "BackupTasks");

        repository.CreateDirictory(repoPath, "Dir1");
        repository.CreateDirictory(Path.Combine(repoPath, "Dir1"), "Dir2");
        repository.CreateFile(Path.Combine(repoPath, "Dir1"), "File1.txt");
        repository.CreateFile(repoPath, "File0.txt");
        repository.CreateFile(repoPath, "logfile.log");

        var algorithm = new SplitStorageAlgorithm();

        FileLogger logger = new FileLogger(repository, logfile, LogConfiguration.LogWithoutDate);
        var selector = new SelectByNumberRestorePoint(4);

        var backupTask = new BackupTaskExtra(taskPath, "Backup 1", repository, algorithm, selector, new RestorePointDeleter(), logger);

        var backupObject1 = new BackupObject(boPath1);
        var backupObject2 = new BackupObject(boPath2);

        backupTask.AddBackupObject(backupObject1);
        backupTask.AddBackupObject(backupObject2);
        backupTask.CreateRestorePoint();
        Assert.Equal(2, backupTask.RestorePointsList.ElementAt(0).StoragesList.Count);

        backupTask.RemoveBackupObject(backupObject1);
        backupTask.CreateRestorePoint();
        Assert.Equal(1, backupTask.RestorePointsList.ElementAt(1).StoragesList.Count);
    }

    [Fact]
    public void CreateTask_CleaningRestorePointsByDelitingByNumber_CheckRestorePointsNumber()
    {
        string repoPath = @"InMemRepo";
        string logfile = Path.Combine(@"InMemRepo", "logfile.log");
        string taskPath = Path.Combine(@"InMemRepo", "BackupTasks");
        string boPath1 = Path.Combine(@"InMemRepo", "Dir1");
        string boPath2 = Path.Combine(@"InMemRepo", "File0.txt");

        var repository = new InMemoryRepositoryExtension(repoPath);
        repository.CreateDirictory(repoPath, "BackupTasks");

        repository.CreateDirictory(repoPath, "Dir1");
        repository.CreateDirictory(Path.Combine(repoPath, "Dir1"), "Dir2");
        repository.CreateFile(Path.Combine(repoPath, "Dir1"), "File1.txt");
        repository.CreateFile(repoPath, "File0.txt");
        repository.CreateFile(repoPath, "logfile.log");

        var algorithm = new SplitStorageAlgorithm();

        FileLogger logger = new FileLogger(repository, logfile, LogConfiguration.LogWithoutDate);
        var selector = new SelectByNumberRestorePoint(4);

        var backupTask = new BackupTaskExtra(taskPath, "Backup 1", repository, algorithm, selector, new RestorePointDeleter(), logger);

        var backupObject1 = new BackupObject(boPath1);
        var backupObject2 = new BackupObject(boPath2);

        backupTask.AddBackupObject(backupObject1);
        backupTask.AddBackupObject(backupObject2);
        backupTask.CreateRestorePoint();

        backupTask.RemoveBackupObject(backupObject1);
        backupTask.CreateRestorePoint();

        backupTask.AddBackupObject(backupObject1);
        backupTask.CreateRestorePoint();

        backupTask.RemoveBackupObject(backupObject2);
        backupTask.CreateRestorePoint();

        selector.ChangeNumber(2);

        backupTask.CreateRestorePoint();
        Assert.Equal(2, backupTask.Backup.RestorePointsList.Count);
        Assert.Equal(2, backupTask.Backup.AllStoragesList.Count);
    }

    [Fact]
    public void CreateTask_CleaningRestorePointsByMergingByNumber_CheckRestorePointsNumber()
    {
        string repoPath = @"InMemRepo";
        string logfile = Path.Combine(@"InMemRepo", "logfile.log");
        string taskPath = Path.Combine(@"InMemRepo", "BackupTasks");
        string boPath1 = Path.Combine(@"InMemRepo", "Dir1");
        string boPath2 = Path.Combine(@"InMemRepo", "File0.txt");

        var repository = new InMemoryRepositoryExtension(repoPath);
        repository.CreateDirictory(repoPath, "BackupTasks");

        repository.CreateDirictory(repoPath, "Dir1");
        repository.CreateDirictory(Path.Combine(repoPath, "Dir1"), "Dir2");
        repository.CreateFile(Path.Combine(repoPath, "Dir1"), "File1.txt");
        repository.CreateFile(repoPath, "File0.txt");
        repository.CreateFile(repoPath, "logfile.log");

        var algorithm = new SplitStorageAlgorithm();

        FileLogger logger = new FileLogger(repository, logfile, LogConfiguration.LogWithoutDate);
        var selector = new SelectByNumberRestorePoint(4);

        var backupTask = new BackupTaskExtra(taskPath, "Backup 1", repository, algorithm, selector, new RestorePointMerger(), logger);

        var backupObject1 = new BackupObject(boPath1);
        var backupObject2 = new BackupObject(boPath2);

        backupTask.AddBackupObject(backupObject1);
        backupTask.AddBackupObject(backupObject2);
        backupTask.CreateRestorePoint();
        backupTask.RemoveBackupObject(backupObject2);
        backupTask.CreateRestorePoint();
        backupTask.CreateRestorePoint();
        selector.ChangeNumber(2);
        backupTask.CreateRestorePoint();

        Assert.Equal(3, backupTask.Backup.RestorePointsList.Count);
        Assert.Equal(4, backupTask.Backup.AllStoragesList.Count);
    }

    [Fact]
    public void CreateTask_CleaningRestorePointsByDelitingByTime_CheckRestorePointsNumber()
    {
        string repoPath = @"InMemRepo";
        string logfile = Path.Combine(@"InMemRepo", "logfile.log");
        string taskPath = Path.Combine(@"InMemRepo", "BackupTasks");
        string boPath1 = Path.Combine(@"InMemRepo", "Dir1");
        string boPath2 = Path.Combine(@"InMemRepo", "File0.txt");

        var repository = new InMemoryRepositoryExtension(repoPath);
        repository.CreateDirictory(repoPath, "BackupTasks");

        repository.CreateDirictory(repoPath, "Dir1");
        repository.CreateDirictory(Path.Combine(repoPath, "Dir1"), "Dir2");
        repository.CreateFile(Path.Combine(repoPath, "Dir1"), "File1.txt");
        repository.CreateFile(repoPath, "File0.txt");
        repository.CreateFile(repoPath, "logfile.log");

        var algorithm = new SplitStorageAlgorithm();

        FileLogger logger = new FileLogger(repository, logfile, LogConfiguration.LogWithoutDate);
        var selector = new SelectByTimeRestorePoint(DateTime.Now.AddSeconds(1));

        var backupTask = new BackupTaskExtra(taskPath, "Backup 1", repository, algorithm, selector, new RestorePointDeleter(), logger);

        var backupObject1 = new BackupObject(boPath1);
        var backupObject2 = new BackupObject(boPath2);

        backupTask.AddBackupObject(backupObject1);
        backupTask.AddBackupObject(backupObject2);
        backupTask.CreateRestorePoint();

        Thread.Sleep(1000);

        backupTask.RemoveBackupObject(backupObject1);
        backupTask.CreateRestorePoint();

        backupTask.AddBackupObject(backupObject1);
        backupTask.CreateRestorePoint();

        Assert.Equal(2, backupTask.Backup.RestorePointsList.Count);
        Assert.Equal(3, backupTask.Backup.AllStoragesList.Count);
    }

    [Fact]
    public void CreateTask_CleaningRestorePointsByMergingByTime_CheckRestorePointsNumber()
    {
        string repoPath = @"InMemRepo";
        string logfile = Path.Combine(@"InMemRepo", "logfile.log");
        string taskPath = Path.Combine(@"InMemRepo", "BackupTasks");
        string boPath1 = Path.Combine(@"InMemRepo", "Dir1");
        string boPath2 = Path.Combine(@"InMemRepo", "File0.txt");

        var repository = new InMemoryRepositoryExtension(repoPath);
        repository.CreateDirictory(repoPath, "BackupTasks");

        repository.CreateDirictory(repoPath, "Dir1");
        repository.CreateDirictory(Path.Combine(repoPath, "Dir1"), "Dir2");
        repository.CreateFile(Path.Combine(repoPath, "Dir1"), "File1.txt");
        repository.CreateFile(repoPath, "File0.txt");
        repository.CreateFile(repoPath, "logfile.log");

        var algorithm = new SplitStorageAlgorithm();

        FileLogger logger = new FileLogger(repository, logfile, LogConfiguration.LogWithoutDate);
        var selector = new SelectByTimeRestorePoint(DateTime.Now.AddSeconds(1));

        var backupTask = new BackupTaskExtra(taskPath, "Backup 1", repository, algorithm, selector, new RestorePointMerger(), logger);

        var backupObject1 = new BackupObject(boPath1);
        var backupObject2 = new BackupObject(boPath2);

        backupTask.AddBackupObject(backupObject1);
        backupTask.AddBackupObject(backupObject2);
        backupTask.CreateRestorePoint();
        backupTask.RemoveBackupObject(backupObject1);
        backupTask.CreateRestorePoint();

        Thread.Sleep(1000);

        backupTask.AddBackupObject(backupObject1);
        backupTask.CreateRestorePoint();

        Assert.Equal(2, backupTask.Backup.RestorePointsList.Count);
        Assert.Equal(4, backupTask.Backup.AllStoragesList.Count);
    }
}