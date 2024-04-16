namespace Isu.Extra.Exceptions;

public class GroupAlreadyHasLessonException : Exception
{
    public GroupAlreadyHasLessonException(string message)
        : base(message)
    {
    }
}