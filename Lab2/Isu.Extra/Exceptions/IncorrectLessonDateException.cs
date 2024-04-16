namespace Isu.Extra.Exceptions;

public class IncorrectLessonDateException : Exception
{
    public IncorrectLessonDateException(string message)
        : base(message)
    {
    }
}