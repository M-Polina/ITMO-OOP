using Isu.Entities;

namespace Isu.Exceptions;

public class StudentCanNotBeCreatedExeption : Exception
{
    public StudentCanNotBeCreatedExeption(string message)
        : base(message)
    {
    }
}