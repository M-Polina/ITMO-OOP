namespace Isu.Extra.Exceptions;

public class LessonAlreadyExistsException : Exception
{
    public LessonAlreadyExistsException(string message)
        : base(message)
    {
    }
}