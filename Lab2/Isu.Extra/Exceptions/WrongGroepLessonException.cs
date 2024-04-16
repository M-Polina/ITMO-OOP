namespace Isu.Extra.Exceptions;

public class WrongGroepLessonException : Exception
{
    public WrongGroepLessonException(string message)
        : base(message)
    {
    }
}