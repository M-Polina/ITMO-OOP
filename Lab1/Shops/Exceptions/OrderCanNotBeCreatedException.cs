namespace Isu.Exceptions;

public class OrderCanNotBeCreatedException : Exception
{
    public OrderCanNotBeCreatedException(string message)
        : base(message)
    {
    }
}