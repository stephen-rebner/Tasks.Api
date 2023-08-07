using Tasks.Data;

namespace Tasks.Api.Dtos;

public record CreateTaskDto(string Name, int Priority, Status Status);
