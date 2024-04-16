namespace Isu.Exceptions;

public class PersonCanNotBeCreatedException : Exception
{
    public PersonCanNotBeCreatedException(string message)
        : base(message)
    {
    }
}