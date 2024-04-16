using Isu.Entities;

namespace Isu.Exceptions;

public class GroupIsFullExeption : Exception
{
    public GroupIsFullExeption(string message)
        : base(message)
    {
    }
}
