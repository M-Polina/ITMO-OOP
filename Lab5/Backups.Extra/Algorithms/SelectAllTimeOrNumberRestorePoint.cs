using Backups.Extra.Exceptions;
using Backups.Models;

namespace Backups.Extra.Algorithms;

public class SelectAllTimeOrNumberRestorePoint : ISelectRestorePoint
{
    private const int MinNumber = 0;

    public SelectAllTimeOrNumberRestorePoint(int number, DateTime time)
    {
        if (number <= MinNumber)
            throw new BackupsExtraExceptions("Number < 0 while creating SelectAllTimeOrNumberRestorePoint.");

        MaxNumber = number;
        MinTime = time;
    }

    public int MaxNumber { get; }
    public DateTime MinTime { get; }

    public IReadOnlyCollection<RestorePoint> SelectRestorePoints(IReadOnlyCollection<RestorePoint> givenPoints)
    {
        if (givenPoints is null)
            throw new BackupsExtraExceptions("Null list of RP while selecting in SelectAllTimeOrNumberRestorePoint.");

        int numToGetRid = givenPoints.Count - MaxNumber;
        var selectedByNumPoints = givenPoints.Take(numToGetRid).ToList();
        var selectedByTimePoints = givenPoints.Where(point => point.CreationTime < MinTime).ToList();
        var pointsToAdd = selectedByTimePoints.Where(point => selectedByNumPoints.SingleOrDefault(p => p.Id == point.Id) is null);

        return selectedByNumPoints.Concat(pointsToAdd).ToList();
    }
}