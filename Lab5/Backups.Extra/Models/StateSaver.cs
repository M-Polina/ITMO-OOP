using Backups.Extra.Exceptions;
using Backups.Extra.Logger;
using Backups.Models;
using Newtonsoft.Json;

namespace Backups.Extra.Models;

public class StateSaver
{
    private JsonSerializerSettings _serializerOptions;

    public StateSaver(ILogger logger)
    {
        if (logger is null)
            throw new BackupsExtraExceptions("Null logger while creating StateSaver.");

        Logger = logger;
        _serializerOptions = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
        };
    }

    public ILogger Logger { get; }
    public string DirPath { get; } = @"D:\ITMO\2 year\3 semester\OOP\M-Polina\Lab5\SavedState";

    public void SaveState(List<BackupTaskExtra> tasks)
    {
        Logger.Log("Saving current state...");

        if (Directory.Exists(DirPath))
        {
            Directory.Delete(DirPath, true);
            Logger.Log("Old state was deleted.");
        }

        Directory.CreateDirectory(DirPath);

        string filePath;
        for (int i = 0; i < tasks.Count; i++)
        {
            filePath = Path.Combine(DirPath, $"BackupTask_{i}.json");
            string jsonString = JsonConvert.SerializeObject(tasks[i], _serializerOptions);
            File.WriteAllText(filePath, jsonString);
        }

        Logger.Log("State is saved.");
    }

    public List<BackupTaskExtra> LoadState()
    {
        // Logger.Log("Loading state...");
        var tasks = new List<BackupTaskExtra>();

        if (!Directory.Exists(DirPath))
        {
            Logger.Log("No Saved State was found.");
            return tasks;
        }

        string[] files = Directory.GetFiles(DirPath);
        BackupTaskExtra? newTask;
        foreach (string filename in files)
        {
            newTask = JsonConvert.DeserializeObject<BackupTaskExtra>(
                File.ReadAllText(@"D:\ITMO\2 year\3 semester\OOP\M-Polina\Lab5\SavedState\BackupTask_0.json"), _serializerOptions);
            if (newTask is null)
                throw new BackupsExtraExceptions("Task wasn't loaded correctly while Loading State.");
            tasks.Add(newTask);
        }

        Logger.Log("State was loaded without errors.");
        return tasks;
    }
}