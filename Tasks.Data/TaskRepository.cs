namespace Tasks.Data;

public class TaskRepository : ITaskRepository
{
    private IList<Task> _tasks;

    public TaskRepository()
    {
        _tasks = new List<Task>
        {
            new("Do the dishes", 1, Status.NotStarted),
            new("Do the laundry", 2, Status.InProgress),
            new("Do the cooking", 3, Status.Completed)
        };
    }
    
    public Task GetTaskByName(string name)
    {
        return _tasks.FirstOrDefault(t => t.Name == name);
    }
    
    public Task GetCompletedTaskByName(string name)
    {
        return _tasks.FirstOrDefault(t => t.Name == name && t.Status == Status.Completed);
    }
    
    public void AddTask(Task task)
    {
        _tasks.Add(task);
    }
    
    public void RemoveTask(Task taskToRemove)
    {
        _tasks = _tasks.Where(task => task.Name != taskToRemove.Name).ToList();
    }
    
    public void UpdateTask(Task task)
    {
        var existingTask = GetTaskByName(task.Name);
        if (existingTask != null)
        {
            existingTask.Update(task.Priority, task.Status);
        }
    }
    
    public IList<Task> GetAllTasks()
    {
        return _tasks;
    }
}

