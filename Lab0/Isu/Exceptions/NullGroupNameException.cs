using Isu.Models;

namespace Isu.Exceptions;

public class NullGroupNameException : Exception
{
    public NullGroupNameException(string message)
        : base(message)
    {
    }
}