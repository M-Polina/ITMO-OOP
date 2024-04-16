using Backups.Extra.Exceptions;

namespace Backups.Extra.Logger;

public class ConsoleLogger : ILogger
{
    public ConsoleLogger(LogConfiguration logConfiguration)
    {
        LogConfiguration = logConfiguration;
    }

    public LogConfiguration LogConfiguration { get; }

    public void Log(string logInformation)
    {
        if (string.IsNullOrWhiteSpace(logInformation))
            throw new BackupsExtraExceptions("NullOrWhiteSpace logInformation while logging.");

        if (LogConfiguration == LogConfiguration.LogWithDate)
            logInformation = DateTime.Now.ToString("MM.dd.yyyy-HH:mm:ss : ") + logInformation;

        Console.WriteLine(logInformation);
    }
}