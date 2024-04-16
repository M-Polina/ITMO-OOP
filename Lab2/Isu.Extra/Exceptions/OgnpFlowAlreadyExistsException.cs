namespace Isu.Extra.Exceptions;

public class OgnpFlowAlreadyExistsException : Exception
{
    public OgnpFlowAlreadyExistsException(string message)
        : base(message)
    {
    }
}