namespace Isu.Exceptions;

public class InvalidNewPriceException : Exception
{
    public InvalidNewPriceException(string message)
        : base(message)
    {
    }
}