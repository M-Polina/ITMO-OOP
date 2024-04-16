using Backups.Algorithms;
using Backups.Models;

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