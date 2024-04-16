using Backups.Extra.Exceptions;
using Backups.Extra.Unarchiver;
using Backups.Services;
using Newtonsoft.Json;

namespace Backups.Extra.Logger;

public class FileLogger : ILogger
{
    public FileLogger(IRepositoryExtension repository, string path, LogConfiguration logConfiguration)
    {
        if (repository is null)
            throw new BackupsExtraExceptions("Null repository while creating File Logger");
        if (!repository.IsFile(path))
            throw new BackupsExtraExceptions("File doesn't exist while creating FileLogger.");

        Repository = repository;
        LogFilePath = path;
        LogConfiguration = logConfiguration;
    }

    public IRepositoryExtension Repository { get; }
    [JsonProperty("path")]
    public string LogFilePath { get; }
    public LogConfiguration LogConfiguration { get; }

    public void Log(string logInformation)
    {
        if (string.IsNullOrWhiteSpace(logInformation))
            throw new BackupsExtraExceptions("NullOrWhiteSpace logInformation while logging.");

        if (LogConfiguration == LogConfiguration.LogWithDate)
            logInformation = DateTime.Now.ToString("MM.dd.yyyy-HH:mm:ss : ") + logInformation;

        Repository.WriteToFile(LogFilePath, logInformation);
    }
}