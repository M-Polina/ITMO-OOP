using Isu.Entities;

namespace Isu.Exceptions;

public class InvalidGroupException : Exception
{
    public InvalidGroupException(string message, Group<Student> group)
        : base(message)
    {
        Group = group;
    }

    public Group<Student>? Group { get; }
}