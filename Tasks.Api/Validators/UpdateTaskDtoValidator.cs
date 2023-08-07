using FluentValidation;
using Tasks.Api.Dtos;

namespace Tasks.Api.Validators;

public class UpdateTaskDtoValidator : AbstractValidator<UpdateTaskDto>
{
    public UpdateTaskDtoValidator()
    {
        RuleFor(dto => dto.status).IsInEnum();
    }
}