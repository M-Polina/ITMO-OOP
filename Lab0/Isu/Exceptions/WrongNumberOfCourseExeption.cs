namespace Isu.Exceptions;

public class WrongNumberOfCourseExeption : Exception
{
    public WrongNumberOfCourseExeption(string message)
        : base(message)
    {
    }
}