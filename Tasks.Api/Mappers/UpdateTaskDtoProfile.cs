using AutoMapper;
using Tasks.Api.Dtos;
using Task = Tasks.Data.Task;

namespace Tasks.Api.Mappers;

public class UpdateTaskDtoProfile : Profile
{
    public UpdateTaskDtoProfile()
    {
        // add an automapper profile here to map from UpdateTaskDto to Task
        CreateMap<Task, UpdateTaskDto>();
        CreateMap<UpdateTaskDto, Task>()
            .DisableCtorValidation()
            .ForMember(task => task.Name, opt => opt.Ignore());
    }
}