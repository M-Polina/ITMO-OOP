using Isu.Entities;

namespace Isu.Exceptions;

public class StudentIsFoundException : Exception
{
    public StudentIsFoundException(string message)
        : base(message)
    {
    }
}