using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Tasks.Api.Dtos;
using Tasks.Data;
using Task = Tasks.Data.Task;

namespace Tasks.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController : Controller
{
    private readonly ITaskRepository _taskRepository;
    private readonly IMapper _mapper;

    public TaskController(ITaskRepository taskRepository, IMapper mapper)
    {
        _taskRepository = taskRepository;
        _mapper = mapper;
    }
    
    [HttpGet]
    public IActionResult GetTasks()
    {
        var tasks = _taskRepository.GetAllTasks();
        
        return Ok(_mapper.Map<IEnumerable<GetTaskDto>>(tasks));
    }
    
    [HttpGet("{name}", Name = "GetTask")]
    public IActionResult GetTask(string name)
    {
        var task = _taskRepository.GetTaskByName(name);
        if (task == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<GetTaskDto>(task));
    }
    
    [HttpPost]
    public IActionResult CreateTask([FromBody] CreateTaskDto createTaskDto)
    {
        if (createTaskDto == null)
        {
            return BadRequest();
        }

        var taskEntity = _mapper.Map<Task>(createTaskDto);
        
        _taskRepository.AddTask(taskEntity);

        return CreatedAtRoute("GetTask", new { name = createTaskDto.Name }, createTaskDto);
    }
    
    [HttpPut("{name}")]
    public IActionResult UpdateTask(string name, [FromBody] UpdateTaskDto updateTaskDto)
    {
        if (updateTaskDto == null)
        {
            return BadRequest();
        }

        var taskEntity = _taskRepository.GetTaskByName(name);
        if (taskEntity == null)
        {
            return NotFound();
        }

        _mapper.Map(updateTaskDto, taskEntity);
        
        _taskRepository.UpdateTask(taskEntity);

        return NoContent();
    }
    
    [HttpDelete("{name}")]
    public IActionResult DeleteTask(string name)
    {
        var taskEntity = _taskRepository.GetCompletedTaskByName(name);
        if (taskEntity == null)
        {
            return BadRequest();
        }

        _taskRepository.RemoveTask(taskEntity);

        return NoContent();
    }
}