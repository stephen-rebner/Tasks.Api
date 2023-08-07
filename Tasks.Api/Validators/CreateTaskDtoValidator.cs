using FluentValidation;
using Tasks.Api.Dtos;
using Tasks.Data;

namespace Tasks.Api.Validators;

public class CreateTaskDtoValidator : AbstractValidator<CreateTaskDto>
{
    private readonly ITaskRepository _taskRepository;

    public CreateTaskDtoValidator(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
        
        RuleFor(x => x.Name)
            .NotEmpty()
            .Custom((name, context) => IsUniqueName(name, context));
        
        RuleFor(x => x.Status).IsInEnum();
    }
    
    private void IsUniqueName(string name, ValidationContext<CreateTaskDto> validationContext)
    {
        if (_taskRepository.GetTaskByName(name) != null)
        {
            validationContext.AddFailure("Name", "'Name' must be unique");
        }
    }
}