namespace BusinessLayer.Exceptions;

public class BusinessLayerException: Exception
{
    public BusinessLayerException()
    {
    }

    public BusinessLayerException(string message)
        : base(message)
    {
    }

    public BusinessLayerException(string message, Exception inner)
        : base(message, inner)
    {
    }
}