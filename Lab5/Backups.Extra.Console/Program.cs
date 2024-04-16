using Backups.Algorithms;
using Backups.Extra.Algorithms;
using Backups.Extra.Logger;
using Backups.Extra.Models;
using Backups.Extra.Unarchiver;
using Backups.Models;

void Test1()
{
    string repoPath = @"D:\ITMO\2 year\3 semester\OOP\lab5_RestoreObjects\Backups";
    string taskPath = @"D:\ITMO\2 year\3 semester\OOP\lab5_RestoreObjects\Backups\BackupTasks";
    string boPath1 = @"D:\ITMO\2 year\3 semester\OOP\lab5_RestoreObjects\Dir1";
    string boPath3 = @"D:\ITMO\2 year\3 semester\OOP\lab5_RestoreObjects\Dir1\file2.txt";
    string boPath2 = @"D:\ITMO\2 year\3 semester\OOP\lab5_RestoreObjects\file1.txt";

    var repository = new FileSystemRepositoryExtension(repoPath);
    var algorithm = new SplitStorageAlgorithm();

    ConsoleLogger logger = new ConsoleLogger(LogConfiguration.LogWithoutDate);
    var selector = new SelectByNumberRestorePoint(3);

    var backupTask = new BackupTaskExtra(taskPath, "Backup 1", repository, algorithm, selector,
        new RestorePointMerger(),
        logger);

    var backupObject1 = new BackupObject(boPath1);
    var backupObject2 = new BackupObject(boPath2);
    var backupObject3 = new BackupObject(boPath3);


    backupTask.AddBackupObject(backupObject1);
    backupTask.AddBackupObject(backupObject2);
    backupTask.AddBackupObject(backupObject3);

    backupTask.CreateRestorePoint();
// Thread.Sleep(10000);

    backupTask.CreateRestorePoint();
// Thread.Sleep(10000);

    backupTask.CreateRestorePoint();
// Thread.Sleep(10000);

    selector.ChangeNumber(2);
// Thread.Sleep(10000);

    backupTask.RemoveBackupObject(backupObject2);
    backupTask.CreateRestorePoint();
    Thread.Sleep(10000);
    Unarchiver unarchiver = new Unarchiver(repository, logger);
    unarchiver.UnarchiveToDifferentLocation(backupTask.BackupTask.Backup.RestorePointsList.Last(),
        @"D:\ITMO\2 year\3 semester\OOP\lab5_RestoreObjects\Unarchived");
    unarchiver.UnarchiveToOriginalLocation(backupTask.BackupTask.Backup.RestorePointsList.Last());
}

void Test2()
{
    string repoPath = @"D:\ITMO\2 year\3 semester\OOP\lab5_RestoreObjects\Backups";
    string taskPath = @"D:\ITMO\2 year\3 semester\OOP\lab5_RestoreObjects\Backups\BackupTasks";
    string boPath1 = @"D:\ITMO\2 year\3 semester\OOP\lab5_RestoreObjects\Dir1";
    string boPath3 = @"D:\ITMO\2 year\3 semester\OOP\lab5_RestoreObjects\Dir1\file2.txt";
    string boPath2 = @"D:\ITMO\2 year\3 semester\OOP\lab5_RestoreObjects\file1.txt";
    string logFile = @"D:\ITMO\2 year\3 semester\OOP\lab5_RestoreObjects\LogInfo.log";

    var repository = new FileSystemRepositoryExtension(repoPath);
    var algorithm = new SingleStorageAlgorithm();

    FileLogger logger = new FileLogger(repository, logFile, LogConfiguration.LogWithoutDate);
    var selector = new SelectByTimeRestorePoint(DateTime.Now.AddSeconds(1));

    var backupTask = new BackupTaskExtra(taskPath, "Backup 1", repository, algorithm, selector,
        new RestorePointMerger(),
        logger);

    var backupObject1 = new BackupObject(boPath1);
    var backupObject2 = new BackupObject(boPath2);
    var backupObject3 = new BackupObject(boPath3);


    backupTask.AddBackupObject(backupObject1);
    backupTask.AddBackupObject(backupObject3);

    backupTask.CreateRestorePoint();

    backupTask.AddBackupObject(backupObject2);
    repository.WriteToFile(backupObject2.FullPath, "Last");
    backupTask.RemoveBackupObject(backupObject3);
    backupTask.CreateRestorePoint();
    Thread.Sleep(100);

    // backupTask.CreateRestorePoint();

    Unarchiver unarchiver = new Unarchiver(repository, logger);
    unarchiver.UnarchiveToDifferentLocation(backupTask.BackupTask.Backup.RestorePointsList.Last(),
        @"D:\ITMO\2 year\3 semester\OOP\lab5_RestoreObjects\Unarchived");
    unarchiver.UnarchiveToOriginalLocation(backupTask.BackupTask.Backup.RestorePointsList.Last());
    
    StateSaver saver = new StateSaver(logger);
    saver.SaveState(new List<BackupTaskExtra>() { backupTask });
}

void Test3()
{
    ConsoleLogger logger = new ConsoleLogger(LogConfiguration.LogWithoutDate);
    StateSaver saver = new StateSaver(logger);
    var tasks = saver.LoadState();
    Console.WriteLine(tasks.ElementAt(0).Name);
}

Test2();