namespace Isu.Extra.Exceptions;

public class WrongLessonException : Exception
{
    public WrongLessonException(string message)
        : base(message)
    {
    }
}