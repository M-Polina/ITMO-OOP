namespace Isu.Exceptions;

public class ProductPriceCanNotBeChangedException : Exception
{
    public ProductPriceCanNotBeChangedException(string message)
        : base(message)
    {
    }
}