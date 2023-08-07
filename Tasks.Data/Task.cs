namespace Tasks.Data;

public class Task
{

    public int Priority { get; private set; }

    public string Name { get; private set; }
    
    public Status Status { get; private set; }
    
    public Task(string name, int priority, Status status)
    {
        Name = name;
        Priority = priority;
        Status = status;
    }
    
    public void Update(int priority, Status status)
    {
        Priority = priority;
        Status = status;
    }
}


public enum Status
{
    NotStarted,
    InProgress,
    Completed
}