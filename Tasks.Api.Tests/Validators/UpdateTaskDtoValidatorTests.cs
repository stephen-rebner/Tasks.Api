using FluentAssertions;
using Tasks.Api.Dtos;
using Tasks.Api.Validators;
using Tasks.Data;

namespace Tasks.Api.Tests.Validators;

public class UpdateTaskDtoValidatorTests
{
    [Test]
    public void UpdateTaskDtoValidator_WhenStatusIsNotValid_ReturnsValidationError()
    {
        // arrange
        const Status invalidStatus = (Status)100;
        
        var validator = new UpdateTaskDtoValidator();
        var dto = new UpdateTaskDto(1, invalidStatus);

        // act
        var result = validator.Validate(dto);

        // assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle();
        result.Errors.First().PropertyName.Should().Be("status");
        result.Errors.First().ErrorMessage.Should().Be($"'status' has a range of values which does not include '{invalidStatus}'.");
    }
    
    [Test]
    public void UpdateTaskDtoValidator_WhenAllPropertiesAreValid_ReturnsValid()
    {
        // arrange
        var validator = new UpdateTaskDtoValidator();
        var dto = new UpdateTaskDto(1, Status.InProgress);

        // act
        var result = validator.Validate(dto);

        // assert
        result.IsValid.Should().BeTrue();
    }
    
}