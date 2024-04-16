namespace Isu.Exceptions;

public class ShopProductCanNotBeCreatedException : Exception
{
    public ShopProductCanNotBeCreatedException(string message)
        : base(message)
    {
    }
}