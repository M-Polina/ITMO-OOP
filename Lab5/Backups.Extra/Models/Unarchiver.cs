using Backups.Algorithms;
using Backups.Extra.Exceptions;
using Backups.Extra.Logger;
using Backups.Extra.Unarchiver;
using Backups.Models;

namespace Backups.Extra.Models;

public class Unarchiver
{
    public Unarchiver(IRepositoryExtension repository, ILogger logger)
    {
        if (repository is null)
            throw new BackupsExtraExceptions("Null IRepositoryExtension while creating unarchiver.");
        if (logger is null)
            throw new BackupsExtraExceptions("Null logger while creating unarchiver.");

        Repository = repository;
        Logger = logger;
    }

    public IRepositoryExtension Repository { get; }
    public ILogger Logger { get; }

    public void UnarchiveToOriginalLocation(RestorePoint restorePoint)
    {
        if (restorePoint is null)
            throw new BackupsExtraExceptions("Null restore point while unarchivig.");

        foreach (var st in restorePoint.StoragesList)
        {
            if (st.Algorithm is SplitStorageAlgorithm)
            {
                Repository.UnarchiveToDirictory(st.Path, Repository.GetStringDirectoryOfFileOrDir(st.BackupObjectsList.ElementAt(0).FullPath));
            }

            if (st.Algorithm is SingleStorageAlgorithm)
            {
                string seviceDir = Path.Combine(restorePoint.FullPath, "SeviceDir");
                Repository.UnarchiveToDirictory(st.Path, seviceDir);

                foreach (var bo in st.BackupObjectsList)
                {
                    Repository.CopyObject(Path.Combine(seviceDir, bo.Name), Repository.GetStringDirectoryOfFileOrDir(bo.FullPath), bo.FullPath);
                }

                Repository.DeliteDirectoryRecursively(seviceDir);
            }
        }

        Logger.Log("Restore point was unarchived.");
    }

    public void UnarchiveToDifferentLocation(RestorePoint restorePoint, string unurchivePath)
    {
        if (restorePoint is null)
            throw new BackupsExtraExceptions("Null restore point while unarchivig.");

        foreach (var st in restorePoint.StoragesList)
        {
            Repository.UnarchiveToDirictory(st.Path, unurchivePath);
        }

        Logger.Log("Restore point was unarchived.");
    }
}