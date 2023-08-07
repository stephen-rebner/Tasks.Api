using AutoMapper;
using Tasks.Api.Dtos;
using Task = Tasks.Data.Task;

namespace Tasks.Api.Mappers;

public class GetTaskDtoProfile : Profile
{
    public GetTaskDtoProfile()
    {
        CreateMap<Task, GetTaskDto>();
        CreateMap<GetTaskDto, Task>();
    }
}