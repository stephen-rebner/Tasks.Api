using FluentAssertions;
using Moq;
using Tasks.Api.Dtos;
using Tasks.Api.Validators;
using Tasks.Data;
using Task = Tasks.Data.Task;

namespace Tasks.Api.Tests.Validators;

public class CreateTaskDtoValidatorTests
{
    
    [Test]
    public void CreateTaskDtoValidator_ShouldPassValidation_WhenTaskNameIsUniqueAndNameIsNotEmpty()
    {
        // arrange
        var taskRepository = new Mock<ITaskRepository>();
        taskRepository.Setup(x => x.GetTaskByName(It.IsAny<string>())).Returns((Task)null);
        
        var taskDtoValidator = new CreateTaskDtoValidator(taskRepository.Object);
        var taskDto = new CreateTaskDto("Do the dishes", 1, Status.InProgress);

        // act
        var validationResult = taskDtoValidator.Validate(taskDto);
        
        // assert
        validationResult.IsValid.Should().BeTrue();
    }
    
    // add an nunit test that checks that validation fails when the task name is not unique
    [Test]
    public void CreateTaskDtoValidator_ShouldFailValidation_WhenTaskNameIsNotUnique()
    {
        // arrange
        var task = new Task("Do the dishes", 1, Status.InProgress);
        
        var taskRepository = new Mock<ITaskRepository>();
        taskRepository.Setup(x => x.GetTaskByName(It.IsAny<string>()))
            .Returns(task);
        
        var taskDtoValidator = new CreateTaskDtoValidator(taskRepository.Object);
        var taskDto = new CreateTaskDto(task.Name, task.Priority, task.Status);

        // act
        var validationResult = taskDtoValidator.Validate(taskDto);
        
        // assert
        validationResult.IsValid.Should().BeFalse();
    }
    
    // add an nunit test that checks that validation fails when the task name is empty
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void CreateTaskDtoValidator_ShouldFailValidation_WhenTaskNameIsEmpty(string emptyName)
    {
        // arrange
        var taskRepository = new Mock<ITaskRepository>();
        taskRepository.Setup(x => x.GetTaskByName(It.IsAny<string>())).Returns((Task)null);
        
        var taskDtoValidator = new CreateTaskDtoValidator(taskRepository.Object);
        var taskDto = new CreateTaskDto(emptyName, 1, Status.InProgress);

        // act
        var validationResult = taskDtoValidator.Validate(taskDto);
        
        // assert
        validationResult.IsValid.Should().BeFalse();
    }
    
    // add an nunit test that checks that validation fails when the task status is not valid
    [Test]
    public void CreateTaskDtoValidator_ShouldFailValidation_WhenTaskStatusIsNotValid()
    {
        // arrange
        var taskRepository = new Mock<ITaskRepository>();
        taskRepository.Setup(x => x.GetTaskByName(It.IsAny<string>())).Returns((Task)null);
        
        var taskDtoValidator = new CreateTaskDtoValidator(taskRepository.Object);
        var taskDto = new CreateTaskDto("Do the dishes", 1, (Status)100);

        // act
        var validationResult = taskDtoValidator.Validate(taskDto);
        
        // assert
        validationResult.IsValid.Should().BeFalse();
    }
    
    // add an nunit test that checks the error message is correct when task is not unique
    [Test]
    public void CreateTaskDtoValidator_ShouldReturnCorrectErrorMessage_WhenTaskNameIsNotUnique()
    {
        // arrange
        var task = new Task("Do the dishes", 1, Status.InProgress);
        
        var taskRepository = new Mock<ITaskRepository>();
        taskRepository.Setup(x => x.GetTaskByName(It.IsAny<string>()))
            .Returns(task);
        
        var taskDtoValidator = new CreateTaskDtoValidator(taskRepository.Object);
        var taskDto = new CreateTaskDto(task.Name, task.Priority, task.Status);

        // act
        var validationResult = taskDtoValidator.Validate(taskDto);
        
        // assert
        validationResult.Errors.Should().ContainSingle();
        validationResult.Errors.First().ErrorMessage.Should().Be("'Name' must be unique");
    }
    
    // add an nunit test that checks the error message is correct when task name is empty
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void CreateTaskDtoValidator_ShouldReturnCorrectErrorMessage_WhenTaskNameIsEmpty(string emptyName)
    {
        // arrange
        var taskRepository = new Mock<ITaskRepository>();
        taskRepository.Setup(x => x.GetTaskByName(It.IsAny<string>())).Returns((Task)null);
        
        var taskDtoValidator = new CreateTaskDtoValidator(taskRepository.Object);
        var taskDto = new CreateTaskDto(emptyName, 1, Status.InProgress);

        // act
        var validationResult = taskDtoValidator.Validate(taskDto);
        
        // assert
        validationResult.Errors.Should().ContainSingle();
        validationResult.Errors.First().ErrorMessage.Should().Be("'Name' must not be empty.");
    }
    
    // add an nunit test that checks the error message is correct when task status is not valid
    [Test]
    public void CreateTaskDtoValidator_ShouldReturnCorrectErrorMessage_WhenTaskStatusIsNotValid()
    {
        // arrange
        var taskRepository = new Mock<ITaskRepository>();
        taskRepository.Setup(x => x.GetTaskByName(It.IsAny<string>())).Returns((Task)null);
        
        var taskDtoValidator = new CreateTaskDtoValidator(taskRepository.Object);
        var taskDto = new CreateTaskDto("Do the dishes", 1, (Status)100);

        // act
        var validationResult = taskDtoValidator.Validate(taskDto);
        
        // assert
        validationResult.Errors.Should().ContainSingle();
        validationResult.Errors.First().ErrorMessage.Should().Be("'Status' has a range of values which does not include '100'.");
    }
    
    // add an nunit test that checks the error messages are correct when task name is empty and task status is not valid
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void CreateTaskDtoValidator_ShouldReturnCorrectErrorMessages_WhenTaskNameIsEmptyAndTaskStatusIsNotValid(string emptyName)
    {
        // arrange
        var taskRepository = new Mock<ITaskRepository>();
        taskRepository.Setup(x => x.GetTaskByName(It.IsAny<string>())).Returns((Task)null);
        
        var taskDtoValidator = new CreateTaskDtoValidator(taskRepository.Object);
        var taskDto = new CreateTaskDto(emptyName, 1, (Status)100);

        // act
        var validationResult = taskDtoValidator.Validate(taskDto);
        
        // assert
        validationResult.Errors.Should().HaveCount(2);
        validationResult.Errors.First().ErrorMessage.Should().Be("'Name' must not be empty.");
        validationResult.Errors.Last().ErrorMessage.Should().Be("'Status' has a range of values which does not include '100'.");
    }
    
    // add an nunit test that checks the error messages are correct when task name is not unique and task status is not valid
    [Test]
    public void CreateTaskDtoValidator_ShouldReturnCorrectErrorMessages_WhenTaskNameIsNotUniqueAndTaskStatusIsNotValid()
    {
        // arrange
        var task = new Task("Do the dishes", 1, (Status)100);
        
        var taskRepository = new Mock<ITaskRepository>();
        taskRepository.Setup(x => x.GetTaskByName(It.IsAny<string>())).Returns(task);
        
        var taskDtoValidator = new CreateTaskDtoValidator(taskRepository.Object);
        var taskDto = new CreateTaskDto(task.Name, task.Priority, task.Status);

        // act
        var validationResult = taskDtoValidator.Validate(taskDto);
        
        // assert
        validationResult.Errors.Should().HaveCount(2);
        validationResult.Errors.First().ErrorMessage.Should().Be("'Name' must be unique");
        validationResult.Errors.Last().ErrorMessage.Should().Be("'Status' has a range of values which does not include '100'.");
    }
}