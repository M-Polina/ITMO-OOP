using Isu.Entities;

namespace Isu.Exceptions;

public class GroupIsEmptyExeption : Exception
{
    public GroupIsEmptyExeption(string message)
        : base(message)
    {
    }
}