using System.Net;
using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Tasks.Api.Dtos;
using Tasks.Data;
using Task = System.Threading.Tasks.Task;

namespace Tasks.Api.IntegrationTests;

public class TaskControllerTests
{
    [Test]
    public async Task GetAllTasks_Returns200Ok()
    {
        // SetUp
        var webAppFactory = new WebApplicationFactory<Program>();

        var client = webAppFactory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/task");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public async Task GetTask_Returns200Ok()
    {
        // SetUp
        var webAppFactory = new WebApplicationFactory<Program>();
        var taskRepository = webAppFactory.Services.GetService<ITaskRepository>();

        var task = taskRepository.GetAllTasks().First();

        var client = webAppFactory.CreateClient();

        // Act
        var response = await client.GetAsync($"/api/task/{task.Name}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public async Task GetTask_Returns404NotFound_WhenATaskIsNotFound()
    {
        // SetUp
        var webAppFactory = new WebApplicationFactory<Program>();
        var client = webAppFactory.CreateClient();

        // Act
        var response = await client.GetAsync($"/api/task/doesnotexist");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task CreateTask_Returns201Created_WhenATaskIsCreated()
    {
        // SetUp
        var webAppFactory = new WebApplicationFactory<Program>();
        var client = webAppFactory.CreateClient();

        var task = new Tasks.Data.Task("New Task", 0, Status.InProgress);

        // Act
        var response = await client.PostAsync($"/api/task",
            new StringContent(JsonConvert.SerializeObject(new CreateTaskDto(task.Name, task.Priority, task.Status)), Encoding.UTF8, "application/json"));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Test]
    public async Task CreateTask_Returns400BadRequest_WhenANullTaskIsPassed()
    {
        // SetUp
        var webAppFactory = new WebApplicationFactory<Program>();
        var client = webAppFactory.CreateClient();

        // Act
        var response = await client.PostAsync($"/api/task",
            new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json"));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task CreateTask_Returns400BadRequest_WhenAnEmptyTaskIsPassed()
    {
        // SetUp
        var webAppFactory = new WebApplicationFactory<Program>();
        var client = webAppFactory.CreateClient();

        // Act
        var response = await client.PostAsync($"/api/task",
            new StringContent(JsonConvert.SerializeObject(new CreateTaskDto(default, default, default)),
                Encoding.UTF8, "application/json"));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task CreateTask_Returns400BadRequest_WhenADuplicateTaskIsPassed()
    {
        // SetUp
        var webAppFactory = new WebApplicationFactory<Program>();
        var taskRepository = webAppFactory.Services.GetService<ITaskRepository>();

        var client = webAppFactory.CreateClient();

        var task = taskRepository.GetAllTasks().First();

        // Act
        var response = await client.PostAsync($"/api/task",
            new StringContent(JsonConvert.SerializeObject(new CreateTaskDto(task.Name, task.Priority, task.Status)), Encoding.UTF8, "application/json"));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task UpdateTask_Returns200Ok_WhenAValidTaskIsPassed()
    {
        // SetUp
        var webAppFactory = new WebApplicationFactory<Program>();
        var taskRepository = webAppFactory.Services.GetService<ITaskRepository>();

        var client = webAppFactory.CreateClient();

        var task = taskRepository.GetAllTasks().First();

        // Act
        var response = await client.PutAsync($"/api/task/{task.Name}",
            new StringContent(JsonConvert.SerializeObject(
                    new UpdateTaskDto(task.Priority, task.Status)), 
                Encoding.UTF8, 
                "application/json"));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Test]
    public async Task UpdateTask_Returns400BadRequest_WhenUpdatingATaskWithAnInvalidStatus()
    {
        // SetUp
        const Status invalidStatus = (Status) 100;
        
        var webAppFactory = new WebApplicationFactory<Program>();
        var taskRepository = webAppFactory.Services.GetService<ITaskRepository>();

        var client = webAppFactory.CreateClient();

        var task = taskRepository.GetAllTasks().First();

        // Act
        var response = await client.PutAsync($"/api/task/{task.Name}",
            new StringContent(JsonConvert.SerializeObject(
                    new UpdateTaskDto(task.Priority, invalidStatus)), 
                Encoding.UTF8, 
                "application/json"));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Test]
    public async Task DeleteTask_Returns204NoContent_WhenDeletingATask()
    {
        // SetUp
        var webAppFactory = new WebApplicationFactory<Program>();
        var taskRepository = webAppFactory.Services.GetService<ITaskRepository>();

        var client = webAppFactory.CreateClient();

        var task = taskRepository.GetAllTasks().First(task => task.Status == Status.Completed);

        // Act
        var response = await client.DeleteAsync($"/api/task/{task.Name}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Test]
    public async Task DeleteTask_Returns404NotFound_WhenDeletingATaskThatDoesNotExist()
    {
        // SetUp
        var webAppFactory = new WebApplicationFactory<Program>();
        var client = webAppFactory.CreateClient();

        // Act
        var response = await client.DeleteAsync($"/api/task/doesnotexist");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task DeleteTask_Returns400BadRequest_WhenDeletingATaskThatIsNotCompleted()
    {
        // SetUp
        var webAppFactory = new WebApplicationFactory<Program>();
        var taskRepository = webAppFactory.Services.GetService<ITaskRepository>();

        var client = webAppFactory.CreateClient();

        var task = taskRepository.GetAllTasks().First(task => task.Status != Status.Completed);

        // Act
        var response = await client.DeleteAsync($"/api/task/{task.Name}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}