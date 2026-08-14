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

    public TaskItem Add(string title, Priority priority = Priority.Medium, DateTime? dueDate = null, string? category = null)
    {
        var newId = Tasks.Count == 0 ? 1 : Tasks.Max(t => t.Id) + 1;
        var task = new TaskItem { Id = newId, Title = title, Done = false, Priority = priority, DueDate = dueDate, Category = category };
        Tasks.Add(task);
        Save();
        return task;
    }

    public bool Edit(int id, string? title = null, Priority? priority = null, DateTime? dueDate = null, string? category = null)
    {
        var task = Tasks.FirstOrDefault(t => t.Id == id);
        if (task is null) return false;

        if (title is not null) task.Title = title;
        if (priority is not null) task.Priority = priority.Value;
        if (dueDate is not null) task.DueDate = dueDate;
        if (category is not null) task.Category = category;

        Save();
        return true;
    }

    public IEnumerable<TaskItem> Search(string keyword) =>
        Tasks.Where(t => t.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase));

    public IEnumerable<TaskItem> Filter(bool? done = null, string? category = null, Priority? priority = null)
    {
        var query = Tasks.AsEnumerable();
        if (done is not null) query = query.Where(t => t.Done == done);
        if (category is not null) query = query.Where(t => string.Equals(t.Category, category, StringComparison.OrdinalIgnoreCase));
        if (priority is not null) query = query.Where(t => t.Priority == priority);
        return query;
    }

    public IEnumerable<TaskItem> SortByDueDate() =>
        Tasks.OrderBy(t => t.DueDate ?? DateTime.MaxValue);

    public IEnumerable<TaskItem> SortByPriority() =>
        Tasks.OrderByDescending(t => t.Priority);

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
