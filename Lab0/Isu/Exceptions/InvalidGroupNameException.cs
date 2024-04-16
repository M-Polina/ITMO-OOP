using Isu.Models;

namespace Isu.Exceptions;

public class InvalidGroupNameException : Exception
{
    public InvalidGroupNameException(string message, GroupName group)
        : base(message)
    {
    }

    public InvalidGroupNameException(GroupName group, string stringName)
        : this($"Wrong name format. Group {stringName} can't be created", group)
    {
    }
}