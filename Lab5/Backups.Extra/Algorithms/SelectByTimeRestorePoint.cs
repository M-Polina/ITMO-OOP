using Backups.Extra.Exceptions;
using Backups.Models;

namespace Backups.Extra.Algorithms;

public class SelectByTimeRestorePoint : ISelectRestorePoint
{
    public SelectByTimeRestorePoint(DateTime time)
    {
        MinTime = time;
    }

    public DateTime MinTime { get; }

    public IReadOnlyCollection<RestorePoint> SelectRestorePoints(IReadOnlyCollection<RestorePoint> givenPoints)
    {
        if (givenPoints is null)
            throw new BackupsExtraExceptions("Null list of RP while selecting in SelectByTimeRestorePoint.");

        return givenPoints.Where(point => point.CreationTime < MinTime).ToList();
    }
}