namespace Isu.Extra.Exceptions;

public class EmptyTimetableException : Exception
{
    public EmptyTimetableException(string message)
        : base(message)
    {
    }
}