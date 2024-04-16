namespace Isu.Exceptions;

public class AddressCanNotBeCreatedException : Exception
{
    public AddressCanNotBeCreatedException(string message)
        : base(message)
    {
    }
}