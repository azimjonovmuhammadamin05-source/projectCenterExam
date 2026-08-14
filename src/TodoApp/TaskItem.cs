namespace TodoApp;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool Done { get; set; }

    public override string ToString()
    {
        var status = Done ? "x" : " ";
        return $"[{status}] {Id}: {Title}";
    }
}
