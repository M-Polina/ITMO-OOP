using Backups.Exceptions;
using Backups.Extra.Exceptions;
using Backups.Models;
using Newtonsoft.Json;

namespace Backups.Extra.Algorithms;

public class SelectByNumberRestorePoint : ISelectRestorePoint
{
    private const int MinNumber = 0;

    public SelectByNumberRestorePoint(int number)
    {
        if (number <= MinNumber)
            throw new BackupsExtraExceptions("Number <= 0 while creating SelectByNumberRestorePoint.");

        MaxNumber = number;
    }

    [JsonProperty("number")]
    public int MaxNumber { get; private set; }

    public void ChangeNumber(int n)
    {
        if (n <= MinNumber)
            throw new BackupsExtraExceptions("Wrong new selection number.");
        MaxNumber = n;
    }

    public IReadOnlyCollection<RestorePoint> SelectRestorePoints(IReadOnlyCollection<RestorePoint> givenPoints)
    {
        if (givenPoints is null)
            throw new BackupsExtraExceptions("Null list of RP while selecting in SelectByNumberRestorePoint.");

        int numToGetRid = givenPoints.Count - MaxNumber;
        return givenPoints.Take(numToGetRid).ToList();
    }
}