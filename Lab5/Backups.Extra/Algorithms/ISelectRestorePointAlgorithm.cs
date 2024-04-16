using Backups.Models;

namespace Backups.Extra.Algorithms;

public interface ISelectRestorePoint
{
    public IReadOnlyCollection<RestorePoint> SelectRestorePoints(IReadOnlyCollection<RestorePoint> givenPoints);
}