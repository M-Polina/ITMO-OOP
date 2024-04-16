namespace Isu.Extra.Exceptions;

public class NullCourseFlowException : Exception
{
    public NullCourseFlowException(string message)
        : base(message)
    {
    }
}