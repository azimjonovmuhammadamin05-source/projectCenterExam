using TodoApp;
using Xunit;

namespace TodoApp.Tests;

public class TaskManagerTests
{
    private static TaskManager CreateManager()
    {
        var tempFile = Path.Combine(Path.GetTempPath(), $"tasks_{Guid.NewGuid()}.json");
        return new TaskManager(tempFile);
    }

    [Fact]
    public void Add_CreatesTaskWithIncrementingId()
    {
        var manager = CreateManager();
        var task = manager.Add("write tests");

        Assert.Equal(1, task.Id);
        Assert.Equal("write tests", task.Title);
        Assert.False(task.Done);
    }

    [Fact]
    public void List_ReturnsAllAddedTasks()
    {
        var manager = CreateManager();
        manager.Add("a");
        manager.Add("b");

        Assert.Equal(2, manager.List().Count());
    }

    [Fact]
    public void Remove_ExistingTask_ReturnsTrueAndRemoves()
    {
        var manager = CreateManager();
        var task = manager.Add("temp");

        var result = manager.Remove(task.Id);

        Assert.True(result);
        Assert.Empty(manager.List());
    }

    [Fact]
    public void Remove_NonexistentTask_ReturnsFalse()
    {
        var manager = CreateManager();

        var result = manager.Remove(999);

        Assert.False(result);
    }

    [Fact]
    public void MarkDone_ExistingTask_SetsDoneTrue()
    {
        var manager = CreateManager();
        var task = manager.Add("finish project");

        var result = manager.MarkDone(task.Id);

        Assert.True(result);
        Assert.True(manager.List().First().Done);
    }

    [Fact]
    public void MarkDone_NonexistentTask_ReturnsFalse()
    {
        var manager = CreateManager();

        var result = manager.MarkDone(999);

        Assert.False(result);
    }
}
