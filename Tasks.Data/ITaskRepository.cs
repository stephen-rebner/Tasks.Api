namespace Tasks.Data;

public interface ITaskRepository
{
    Task GetTaskByName(string name);
    Task GetCompletedTaskByName(string name);
    void AddTask(Task task);
    void RemoveTask(Task taskToRemove);
    void UpdateTask(Task task);
    IList<Task> GetAllTasks();
}