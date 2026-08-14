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

        switch (command)
        {
            case "add":
                if (args.Length < 2) { Console.WriteLine("Usage: add <title>"); return; }
                var title = string.Join(' ', args.Skip(1));
                var task = manager.Add(title);
                Console.WriteLine($"Added: {task}");
                break;

            case "list":
                foreach (var t in manager.List())
                    Console.WriteLine(t);
                break;

            case "remove":
                if (args.Length < 2 || !int.TryParse(args[1], out var removeId))
                {
                    Console.WriteLine("Usage: remove <id>");
                    return;
                }
                Console.WriteLine(manager.Remove(removeId) ? "Removed" : "Not found");
                break;

            case "done":
                if (args.Length < 2 || !int.TryParse(args[1], out var doneId))
                {
                    Console.WriteLine("Usage: done <id>");
                    return;
                }
                Console.WriteLine(manager.MarkDone(doneId) ? "Marked done" : "Not found");
                break;

            default:
                PrintHelp();
                break;
        }
    }

    private static void PrintHelp()
    {
        Console.WriteLine("Todo CLI");
        Console.WriteLine("Usage:");
        Console.WriteLine("  add <title>   Add a new task");
        Console.WriteLine("  list          List all tasks");
        Console.WriteLine("  done <id>     Mark a task as done");
        Console.WriteLine("  remove <id>   Remove a task");
    }
}
