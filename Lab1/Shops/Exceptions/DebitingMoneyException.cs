namespace Isu.Exceptions;

public class DebitingMoneyException : Exception
{
    public DebitingMoneyException(string message)
        : base(message)
    {
    }
}