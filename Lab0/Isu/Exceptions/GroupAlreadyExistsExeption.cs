using Isu.Models;

namespace Isu.Exceptions;

public class GroupAlreadyExistsExeption : Exception
{
    public GroupAlreadyExistsExeption(string message)
        : base(message)
    {
    }
}