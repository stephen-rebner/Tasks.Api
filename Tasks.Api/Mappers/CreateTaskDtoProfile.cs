using AutoMapper;
using Tasks.Api.Dtos;
using Task = Tasks.Data.Task;

namespace Tasks.Api.Mappers;

public class CreateTaskDtoProfile : Profile
{
    public CreateTaskDtoProfile()
    {
        // add an automapper profile here to map from UpdateTaskDto to Task
        CreateMap<Task, CreateTaskDto>();
        CreateMap<CreateTaskDto, Task>();
    }
}