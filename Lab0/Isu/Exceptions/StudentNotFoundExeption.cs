using Isu.Entities;

namespace Isu.Exceptions;

public class StudentNotFoundExeption : Exception
{
    public StudentNotFoundExeption(string message)
        : base(message)
    {
    }
}
