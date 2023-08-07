using Tasks.Data;

namespace Tasks.Api.DependencyInjection;

public static class TaskDependencies
{
    // add a static method to add all dependencies to the service collection
    public static IServiceCollection AddTaskDependencies(this IServiceCollection services)
    {
        // add the task repository
        services.AddSingleton<ITaskRepository, TaskRepository>();
        
        // return the service collection
        return services;
    }
}