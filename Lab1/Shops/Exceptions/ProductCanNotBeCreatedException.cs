namespace Isu.Exceptions;

public class ProductCanNotBeCreatedException : Exception
{
    public ProductCanNotBeCreatedException(string message)
        : base(message)
    {
    }
}