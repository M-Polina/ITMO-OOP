namespace Isu.Exceptions;

public class ShopCanNotBeCreatedException : Exception
{
    public ShopCanNotBeCreatedException(string message)
        : base(message)
    {
    }
}