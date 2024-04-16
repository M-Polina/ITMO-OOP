using Backups.Exceptions;
using Backups.Services;
using Newtonsoft.Json;

namespace Backups.Models;

public class RepoDirectory : RepoObject
{
    [JsonProperty("childrenPrivate")]
    private List<RepoObject> _children = new List<RepoObject>();

    [JsonConstructor]
    public RepoDirectory(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new BackupsException("Null or empty RepoFile path");
        }

        FullPath = path;
        Name = Path.GetFileName(path);
    }

    public override IReadOnlyCollection<RepoObject> Children() => _children;

    public override void AddRepoObject(RepoObject repoObject)
    {
        if (repoObject is null)
            throw new BackupsException("repoObject can't be added to dir, because it is null.");

        _children.Add(repoObject);
    }

    public override void RemoveRepoObject(RepoObject repoObject)
    {
        if (repoObject is null)
            throw new BackupsException("repoObject can't be removed from dir, because it is null.");

        if (_children.SingleOrDefault(ro => ro.FullPath.Equals(repoObject.FullPath)) is null)
            throw new BackupsException("repoObject can't be removed from dir, because it isn't there.");

        _children.Remove(repoObject);
    }
}