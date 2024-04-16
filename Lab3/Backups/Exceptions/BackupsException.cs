namespace Backups.Exceptions;

public class BackupsException : Exception
{
    public BackupsException()
    {
    }

    public BackupsException(string message)
        : base(message)
    {
    }

    public BackupsException(string message, Exception inner)
        : base(message, inner)
    {
    }
}