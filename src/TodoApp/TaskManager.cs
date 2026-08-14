using System.Text.Json;

namespace TodoApp;

public class TaskManager
{
    private readonly string _dataFile;
    public List<TaskItem> Tasks { get; private set; } = new();

    public TaskManager(string dataFile = "tasks.json")
    {
        _dataFile = dataFile;
        Load();
    }

    public void Load()
    {
        if (File.Exists(_dataFile))
        {
            var json = File.ReadAllText(_dataFile);
            Tasks = JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
        }
    }

    public void Save()
    {
        var json = JsonSerializer.Serialize(Tasks, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_dataFile, json);
    }

    public TaskItem Add(string title)
    {
        var newId = Tasks.Count == 0 ? 1 : Tasks.Max(t => t.Id) + 1;
        var task = new TaskItem { Id = newId, Title = title, Done = false };
        Tasks.Add(task);
        Save();
        return task;
    }

    public IEnumerable<TaskItem> List() => Tasks;

    public bool Remove(int id)
    {
        var task = Tasks.FirstOrDefault(t => t.Id == id);
        if (task is null) return false;
        Tasks.Remove(task);
        Save();
        return true;
    }

    public bool MarkDone(int id)
    {
        var task = Tasks.FirstOrDefault(t => t.Id == id);
        if (task is null) return false;
        task.Done = true;
        Save();
        return true;
    }
}
