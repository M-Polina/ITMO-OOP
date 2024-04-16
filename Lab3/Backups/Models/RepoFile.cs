using Backups.Exceptions;
using Backups.Services;
using Newtonsoft.Json;

namespace Backups.Models;

public class RepoFile : RepoObject
{
    [JsonProperty("childrenPrivate")]
    private List<RepoObject> _children = new List<RepoObject>();

    [JsonConstructor]
    public RepoFile(string path)
    {
        string name = Path.GetFileName(path);
        if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(name))
        {
            throw new BackupsException("Null or empty RepoFile path or name");
        }

        FullPath = path;
        Name = name;
    }

    public override IReadOnlyCollection<RepoObject> Children() => _children;
}