namespace Backups.Extra.Exceptions;

public class BackupsExtraExceptions : Exception
{
    public BackupsExtraExceptions()
    {
    }

    public BackupsExtraExceptions(string message)
        : base(message)
    {
    }

    public BackupsExtraExceptions(string message, Exception inner)
        : base(message, inner)
    {
    }
}