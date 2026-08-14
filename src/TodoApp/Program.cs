using System.Globalization;

namespace TodoApp;

public static class Program
{
    public static void Main(string[] args)
    {
        var manager = new TaskManager();

        if (args.Length == 0)
        {
            PrintHelp();
            return;
        }

        var command = args[0];
        var rest = args.Skip(1).ToArray();

        switch (command)
        {
            case "add":
                HandleAdd(manager, rest);
                break;

            case "list":
                HandleList(manager, rest);
                break;

            case "edit":
                HandleEdit(manager, rest);
                break;

            case "remove":
                if (rest.Length < 1 || !int.TryParse(rest[0], out var removeId))
                {
                    Console.WriteLine("Usage: remove <id>");
                    return;
                }
                Console.WriteLine(manager.Remove(removeId) ? "Removed" : "Not found");
                break;

            case "done":
                if (rest.Length < 1 || !int.TryParse(rest[0], out var doneId))
                {
                    Console.WriteLine("Usage: done <id>");
                    return;
                }
                Console.WriteLine(manager.MarkDone(doneId) ? "Marked done" : "Not found");
                break;

            case "search":
                if (rest.Length < 1) { Console.WriteLine("Usage: search <keyword>"); return; }
                PrintTasks(manager.Search(rest[0]));
                break;

            default:
                PrintHelp();
                break;
        }
    }

    private static void HandleAdd(TaskManager manager, string[] args)
    {
        if (args.Length < 1) { Console.WriteLine("Usage: add <title> [--priority Low|Medium|High] [--due yyyy-MM-dd] [--category name]"); return; }

        var (title, priority, due, category) = ParseTaskArgs(args);
        var task = manager.Add(title, priority ?? Priority.Medium, due, category);
        PrintTask(task);
    }

    private static void HandleEdit(TaskManager manager, string[] args)
    {
        if (args.Length < 1 || !int.TryParse(args[0], out var id))
        {
            Console.WriteLine("Usage: edit <id> [title] [--priority Low|Medium|High] [--due yyyy-MM-dd] [--category name]");
            return;
        }

        var (title, priority, due, category) = ParseTaskArgs(args.Skip(1).ToArray(), titleOptional: true);
        var ok = manager.Edit(id, string.IsNullOrEmpty(title) ? null : title, priority, due, category);
        Console.WriteLine(ok ? "Updated" : "Not found");
    }

    private static void HandleList(TaskManager manager, string[] args)
    {
        IEnumerable<TaskItem> tasks = manager.List();

        if (args.Contains("--sort-due"))
            tasks = manager.SortByDueDate();
        else if (args.Contains("--sort-priority"))
            tasks = manager.SortByPriority();

        var categoryIdx = Array.IndexOf(args, "--category");
        if (categoryIdx >= 0 && categoryIdx + 1 < args.Length)
            tasks = tasks.Where(t => string.Equals(t.Category, args[categoryIdx + 1], StringComparison.OrdinalIgnoreCase));

        if (args.Contains("--done"))
            tasks = tasks.Where(t => t.Done);
        else if (args.Contains("--pending"))
            tasks = tasks.Where(t => !t.Done);

        PrintTasks(tasks);
    }

    private static (string title, Priority? priority, DateTime? due, string? category) ParseTaskArgs(string[] args, bool titleOptional = false)
    {
        var titleParts = new List<string>();
        Priority? priority = null;
        DateTime? due = null;
        string? category = null;

        for (var i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--priority" when i + 1 < args.Length:
                    if (Enum.TryParse<Priority>(args[i + 1], true, out var p)) priority = p;
                    i++;
                    break;
                case "--due" when i + 1 < args.Length:
                    if (DateTime.TryParse(args[i + 1], CultureInfo.InvariantCulture, DateTimeStyles.None, out var d)) due = d;
                    i++;
                    break;
                case "--category" when i + 1 < args.Length:
                    category = args[i + 1];
                    i++;
                    break;
                default:
                    titleParts.Add(args[i]);
                    break;
            }
        }

        return (string.Join(' ', titleParts), priority, due, category);
    }

    private static void PrintTasks(IEnumerable<TaskItem> tasks)
    {
        foreach (var t in tasks)
            PrintTask(t);
    }

    private static void PrintTask(TaskItem t)
    {
        var originalColor = Console.ForegroundColor;

        if (t.Done)
            Console.ForegroundColor = ConsoleColor.DarkGray;
        else if (t.IsOverdue)
            Console.ForegroundColor = ConsoleColor.Red;
        else if (t.Priority == Priority.High)
            Console.ForegroundColor = ConsoleColor.Yellow;

        Console.WriteLine(t);
        Console.ForegroundColor = originalColor;
    }

    private static void PrintHelp()
    {
        Console.WriteLine("Todo CLI");
        Console.WriteLine("Usage:");
        Console.WriteLine("  add <title> [--priority Low|Medium|High] [--due yyyy-MM-dd] [--category name]");
        Console.WriteLine("  list [--done|--pending] [--category name] [--sort-due|--sort-priority]");
        Console.WriteLine("  edit <id> [title] [--priority ...] [--due ...] [--category ...]");
        Console.WriteLine("  done <id>");
        Console.WriteLine("  remove <id>");
        Console.WriteLine("  search <keyword>");
    }
}
