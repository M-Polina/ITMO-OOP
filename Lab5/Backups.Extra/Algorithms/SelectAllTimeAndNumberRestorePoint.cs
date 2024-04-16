using Backups.Extra.Exceptions;
using Backups.Models;

namespace Backups.Extra.Algorithms;

public class SelectAllTimeAndNumberRestorePoint : ISelectRestorePoint
{
    private const int MinNumber = 0;

    public SelectAllTimeAndNumberRestorePoint(int number, DateTime time)
    {
        if (number <= MinNumber)
            throw new BackupsExtraExceptions("Number < 0 while creating SelectAllTimeAndNumberRestorePoint.");

        MaxNumber = number;
        MinTime = time;
    }

    public int MaxNumber { get; }
    public DateTime MinTime { get; }

    public IReadOnlyCollection<RestorePoint> SelectRestorePoints(IReadOnlyCollection<RestorePoint> givenPoints)
    {
        int numToGetRid = givenPoints.Count - MaxNumber;
        var selectedByNumPoints = givenPoints.Take(numToGetRid).ToList();
        return selectedByNumPoints.Where(point => point.CreationTime < MinTime).ToList();
    }
}