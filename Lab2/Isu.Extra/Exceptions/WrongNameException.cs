namespace Isu.Extra.Exceptions;

public class WrongNameException : Exception
{
    public WrongNameException(string m)
        : base(m)
    {
    }
}