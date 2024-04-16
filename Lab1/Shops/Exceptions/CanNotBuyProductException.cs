namespace Isu.Exceptions;

public class CanNotBuyProductException : Exception
{
    public CanNotBuyProductException(string message)
        : base(message)
    {
    }
}