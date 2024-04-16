namespace Isu.Exceptions;

public class ProductCanNotBeRegisteredException : Exception
{
    public ProductCanNotBeRegisteredException(string message)
        : base(message)
    {
    }
}