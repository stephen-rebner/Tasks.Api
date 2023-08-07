using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Tasks.Api.Controllers;
using Tasks.Api.Dtos;
using Tasks.Api.Mappers;
using Tasks.Data;
using Task = Tasks.Data.Task;

namespace Tasks.Api.Tests.Controllers;

public class TaskControllerTests
{
    private Mock<ITaskRepository> _mockRepository;
    private IMapper _mapper;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<ITaskRepository>();

        var mapperConfig = new MapperConfiguration(config => config.AddProfiles(
            new Profile[]
            {
                new CreateTaskDtoProfile(),
                new UpdateTaskDtoProfile(),
                new GetTaskDtoProfile()
            }
            )); 

        _mapper = mapperConfig.CreateMapper();
    }

    [Test]
    public void GetTasks_ReturnsTasks()
    {
        // arrange
        var tasks = new List<Task>
        {
            new("Task 1", 0, Status.NotStarted),
            new("Task 2", 1, Status.InProgress),
            new("Task 3", 2, Status.Completed)
        };

        _mockRepository.Setup(x => x.GetAllTasks()).Returns(tasks);

        // act
        var controller = new TaskController(_mockRepository.Object, _mapper);

        var result = controller.GetTasks();

        // assert
        result.Should().BeAssignableTo<OkObjectResult>();

        var okResult = result as OkObjectResult;

        okResult?.Value.Should().BeAssignableTo<IEnumerable<GetTaskDto>>();

        var model = okResult?.Value as IEnumerable<GetTaskDto>;

        model.Should().HaveCount(tasks.Count);
    }

    [Test]
    public void GetTask_ReturnsTask()
    {
        // arrange
        var task = new Task("Task 1", 0, Status.NotStarted);

        _mockRepository.Setup(x => x.GetTaskByName(task.Name)).Returns(task);

        // act
        var controller = new TaskController(_mockRepository.Object, _mapper);

        var result = controller.GetTask(task.Name);

        // assert
        result.Should().BeAssignableTo<OkObjectResult>();

        var okResult = result as OkObjectResult;

        okResult?.Value.Should().BeAssignableTo<GetTaskDto>();

        var model = okResult?.Value as GetTaskDto;

        model.Should().BeEquivalentTo(task);
    }

    [Test]
    public void GetTask_ReturnsNotFound_WhenTaskDoesNotExist()
    {
        // arrange
        var task = new Task("Task 1", 0, Status.NotStarted);

        _mockRepository.Setup(x => x.GetTaskByName(task.Name)).Returns((Task)null);

        // act
        var controller = new TaskController(_mockRepository.Object, _mapper);

        var result = controller.GetTask(task.Name);

        // assert
        result.Should().BeAssignableTo<NotFoundResult>();
    }

    [Test]
    public void UpdateTask_ReturnsNoContent_WhenTaskUpdated()
    {
        // arrange
        var task = new Task("Task 1", 0, Status.NotStarted);

        _mockRepository.Setup(x => x.GetTaskByName(task.Name)).Returns(task);

        // act
        var controller = new TaskController(_mockRepository.Object, _mapper);

        var result = controller.UpdateTask(task.Name, new UpdateTaskDto(task.Priority, task.Status));

        // assert
        result.Should().BeAssignableTo<NoContentResult>();
    }

    [Test]
    public void UpdateTask_ReturnsBadRequest_WhenTaskIsNull()
    {
        // arrange
        var task = new Task("Task 1", 0, Status.NotStarted);

        _mockRepository.Setup(x => x.GetTaskByName(task.Name)).Returns(task);

        // act
        var controller = new TaskController(_mockRepository.Object, _mapper);

        var result = controller.UpdateTask(null, null);

        // assert
        result.Should().BeAssignableTo<BadRequestResult>();
    }

    [Test]
    public void UpdateTask_ReturnsNotFound_WhenTaskDoesNotExist()
    {
        // arrange
        var task = new Task("Task 1", 0, Status.NotStarted);

        _mockRepository.Setup(x => x.GetTaskByName(task.Name)).Returns((Task)null);

        // act
        var controller = new TaskController(_mockRepository.Object, _mapper);

        var result = controller.UpdateTask(task.Name, new UpdateTaskDto(task.Priority, task.Status));

        // assert
        result.Should().BeAssignableTo<NotFoundResult>();
    }

    [Test]
    public void CreateTask_ReturnsCreatedAtRoute_WhenTaskCreated()
    {
        // arrange
        var task = new Task("Task 1", 0, Status.NotStarted);

        _mockRepository.Setup(x => x.GetTaskByName(task.Name)).Returns((Task)null);

        // act
        var controller = new TaskController(_mockRepository.Object, _mapper);

        var result = controller.CreateTask(new CreateTaskDto(task.Name, task.Priority, task.Status));

        // assert
        result.Should().BeAssignableTo<CreatedAtRouteResult>();

        var createdAtRouteResult = result as CreatedAtRouteResult;

        createdAtRouteResult?.Value.Should().BeAssignableTo<CreateTaskDto>();

        var model = createdAtRouteResult?.Value as CreateTaskDto;

        model.Should().BeEquivalentTo(task);
    }

    [Test]
    public void CreateTask_ReturnsBadRequest_WhenTaskIsNull()
    {
        // arrange
        var task = new Task("Task 1", 0, Status.NotStarted);

        _mockRepository.Setup(x => x.GetTaskByName(task.Name)).Returns((Task)null);

        // act
        var controller = new TaskController(_mockRepository.Object, _mapper);

        var result = controller.CreateTask(null);

        // assert
        result.Should().BeAssignableTo<BadRequestResult>();
    }

    [Test]
    public void DeleteTask_ReturnsNoContent_WhenTaskDeleted()
    {
        // arrange
        var task = new Task("Task 1", 0, Status.Completed);

        _mockRepository.Setup(x => x.GetCompletedTaskByName(task.Name)).Returns(task);

        // act
        var controller = new TaskController(_mockRepository.Object, _mapper);

        var result = controller.DeleteTask(task.Name);

        // assert
        result.Should().BeAssignableTo<NoContentResult>();
    }

    [Test]
    public void DeleteTask_ReturnsBadRequest_WhenTaskDoesNotExist()
    {
        // arrange
        var task = new Task("Task 1", 0, Status.NotStarted);

        _mockRepository.Setup(x => x.GetCompletedTaskByName(task.Name)).Returns((Task)null);

        // act
        var controller = new TaskController(_mockRepository.Object, _mapper);

        var result = controller.DeleteTask(task.Name);

        // assert
        result.Should().BeAssignableTo<BadRequestResult>();
    }
}