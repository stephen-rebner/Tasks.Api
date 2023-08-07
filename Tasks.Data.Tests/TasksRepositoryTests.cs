using FluentAssertions;

namespace Tasks.Data.Tests;

public class TasksRepositoryTests
{
    [Test]
    public void GetTaskByName_ReturnsATask_WhenATaskExists()
    {
        // Arrange
        var taskRepository = new TaskRepository();
        var expectedTask = new Task("Do the dishes", 1, Status.NotStarted);
        
        // Act
        var actualTask = taskRepository.GetTaskByName("Do the dishes");
        
        // Assert
        actualTask.Should().BeEquivalentTo(expectedTask);
    }
    
    [Test]
    public void GetTaskByName_ReturnsNull_WhenATaskDoesNotExist()
    {
        // Arrange
        var taskRepository = new TaskRepository();
        
        // Act
        var actualTask = taskRepository.GetTaskByName("Does not exist");
        
        // Assert
        actualTask.Should().BeNull();
    }
    
    [Test]
    public void GetCompletedTaskByName_ReturnsATask_WhenATaskExists()
    {
        // Arrange
        var taskRepository = new TaskRepository();
        var expectedTask = new Task("Do the cooking", 3, Status.Completed);
        
        // Act
        var actualTask = taskRepository.GetCompletedTaskByName("Do the cooking");
        
        // Assert
        actualTask.Should().BeEquivalentTo(expectedTask);
    }
    
    [Test]
    public void GetCompletedTaskByName_ReturnsNull_WhenATaskDoesNotExist()
    {
        // Arrange
        var taskRepository = new TaskRepository();
        
        // Act
        var actualTask = taskRepository.GetCompletedTaskByName("Does not exist");
        
        // Assert
        actualTask.Should().BeNull();
    }
    
    [Test]
    public void AddTask_AddsATask_WhenATaskDoesNotExist()
    {
        // Arrange
        var taskRepository = new TaskRepository();
        var task = new Task("Do the shopping1", 4, Status.NotStarted);
        
        // Act
        taskRepository.AddTask(task);
        
        // Assert
        taskRepository.GetAllTasks().Should().Contain(task);
    }
    
    // add an nunit test for addtask that checks the count of tasks before and after adding a task
    [Test]
    public void AddTask_IncreasesTheCountOfTasks_WhenATaskDoesNotExist()
    {
        // Arrange
        var taskRepository = new TaskRepository();
        var task = new Task("Do the shopping2", 4, Status.NotStarted);
        var countBefore = taskRepository.GetAllTasks().Count;
        
        // Act
        taskRepository.AddTask(task);
        var countAfter = taskRepository.GetAllTasks().Count;
        
        // Assert
        countAfter.Should().Be(countBefore+1);
    }
    
    [Test]
    public void RemoveTask_DecreasesTheCountOfTasks_WhenATaskExists()
    {
        // Arrange
        var taskRepository = new TaskRepository();
        var task = new Task("Do the dishes", 1, Status.NotStarted);
        var countBefore = taskRepository.GetAllTasks().Count;
        
        // Act
        taskRepository.RemoveTask(task);
        var countAfter = taskRepository.GetAllTasks().Count;
        
        // Assert
        countAfter.Should().Be(countBefore-1);
    }
    
    [Test]
    public void UpdateTask_DoesNotChangeTheCountOfTasks_WhenATaskExists()
    {
        // Arrange
        var taskRepository = new TaskRepository();
        var task = new Task("Do the dishes", 1, Status.NotStarted);
        var countBefore = taskRepository.GetAllTasks().Count;
        
        // Act
        taskRepository.UpdateTask(task);
        var countAfter = taskRepository.GetAllTasks().Count;
        
        // Assert
        countAfter.Should().Be(countBefore);
    }
    
    [Test]
    public void UpdateTask_UpdatesTheTask_WhenATaskExists()
    {
        // Arrange
        var taskRepository = new TaskRepository();
        
        var updatedTask = new Task("Do the dishes", 2, Status.Completed);
        
        // Act
        taskRepository.UpdateTask(updatedTask);
        var actualTask = taskRepository.GetTaskByName("Do the dishes");
        
        // Assert
        actualTask.Should().BeEquivalentTo(updatedTask);
    }
    
    // add an nunit test for getalltasks 
    [Test] 
    public void GetAllTasks_ReturnsAllTasks()
    {
        // Arrange
        var taskRepository = new TaskRepository();
        var expectedTasks = new List<Task>
        {
            new Task("Do the dishes", 1, Status.NotStarted),
            new Task("Do the laundry", 2, Status.InProgress),
            new Task("Do the cooking", 3, Status.Completed)
        };
        
        // Act
        var actualTasks = taskRepository.GetAllTasks();
        
        // Assert
        actualTasks.Should().BeEquivalentTo(expectedTasks);
    }
}