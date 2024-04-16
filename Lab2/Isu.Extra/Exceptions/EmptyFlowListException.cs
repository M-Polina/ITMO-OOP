namespace Isu.Extra.Exceptions;

public class EmptyFlowListException : Exception
{
    public EmptyFlowListException(string message)
        : base(message)
    {
    }
}