using Backups.Exceptions;

namespace Backups.Services;

public abstract class RepoObject
{
    public string FullPath { get; protected set; } = string.Empty;
    public string Name { get; protected set; } = string.Empty;

    public abstract IReadOnlyCollection<RepoObject> Children();

    public virtual void AddRepoObject(RepoObject repoObject)
    {
        throw new BackupsException("This repoObject is not implementational.");
    }

    public virtual void RemoveRepoObject(RepoObject repoObject)
    {
        throw new BackupsException("This repoObject is not implementational.");
    }
}