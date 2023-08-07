using Tasks.Data;

namespace Tasks.Api.Dtos;

public record UpdateTaskDto(int priority, Status status);