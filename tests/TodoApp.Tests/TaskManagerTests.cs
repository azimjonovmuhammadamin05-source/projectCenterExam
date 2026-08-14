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

    [Fact]
    public void Add_WithPriorityDueDateCategory_SetsFieldsCorrectly()
    {
        var manager = CreateManager();
        var due = new DateTime(2026, 1, 1);

        var task = manager.Add("plan trip", Priority.High, due, "travel");

        Assert.Equal(Priority.High, task.Priority);
        Assert.Equal(due, task.DueDate);
        Assert.Equal("travel", task.Category);
    }

    [Fact]
    public void Edit_UpdatesOnlyProvidedFields()
    {
        var manager = CreateManager();
        var task = manager.Add("old title", Priority.Low);

        var ok = manager.Edit(task.Id, title: "new title");

        Assert.True(ok);
        Assert.Equal("new title", manager.List().First().Title);
        Assert.Equal(Priority.Low, manager.List().First().Priority);
    }

    [Fact]
    public void Search_FindsMatchingTasksCaseInsensitive()
    {
        var manager = CreateManager();
        manager.Add("Buy Milk");
        manager.Add("Walk dog");

        var results = manager.Search("milk").ToList();

        Assert.Single(results);
        Assert.Equal("Buy Milk", results[0].Title);
    }

    [Fact]
    public void Filter_ByCategoryAndDone_ReturnsMatchingTasks()
    {
        var manager = CreateManager();
        var t1 = manager.Add("task1", category: "work");
        manager.Add("task2", category: "home");
        manager.MarkDone(t1.Id);

        var results = manager.Filter(done: true, category: "work").ToList();

        Assert.Single(results);
        Assert.Equal(t1.Id, results[0].Id);
    }

    [Fact]
    public void SortByPriority_OrdersHighFirst()
    {
        var manager = CreateManager();
        manager.Add("low task", Priority.Low);
        manager.Add("high task", Priority.High);
        manager.Add("medium task", Priority.Medium);

        var sorted = manager.SortByPriority().ToList();

        Assert.Equal(Priority.High, sorted[0].Priority);
        Assert.Equal(Priority.Low, sorted[2].Priority);
    }

    [Fact]
    public void IsOverdue_PastDueDateAndNotDone_ReturnsTrue()
    {
        var manager = CreateManager();
        var task = manager.Add("late task", dueDate: DateTime.Today.AddDays(-1));

        Assert.True(task.IsOverdue);
    }
}
