namespace Isu.Exceptions;

public class NullCourseNumberException : Exception
{
    public NullCourseNumberException(string message)
        : base(message)
    {
    }
}