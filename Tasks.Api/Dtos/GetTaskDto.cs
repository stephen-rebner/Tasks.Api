using Tasks.Data;

namespace Tasks.Api.Dtos;

public record GetTaskDto(string Name, int Priority, Status Status);