namespace TodoApp;

public enum Priority
{
    Low,
    Medium,
    High
}

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool Done { get; set; }
    public Priority Priority { get; set; } = Priority.Medium;
    public DateTime? DueDate { get; set; }
    public string? Category { get; set; }

    public bool IsOverdue => !Done && DueDate.HasValue && DueDate.Value.Date < DateTime.Today;

    public override string ToString()
    {
        var status = Done ? "x" : " ";
        var due = DueDate.HasValue ? $" (due {DueDate:yyyy-MM-dd})" : "";
        var cat = !string.IsNullOrEmpty(Category) ? $" #{Category}" : "";
        var overdue = IsOverdue ? " OVERDUE" : "";
        return $"[{status}] {Id}: {Title} [{Priority}]{cat}{due}{overdue}";
    }
}

