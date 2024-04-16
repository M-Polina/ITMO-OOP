namespace Isu.Extra.Exceptions;

public class NullGroupNameException : Exception
{
    public NullGroupNameException(string message)
        : base(message)
    {
    }
}