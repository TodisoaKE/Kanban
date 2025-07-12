using ApprendreDotNet.model.Request.Task;
using ApprendreDotNet.services.task;
using Microsoft.AspNetCore.Mvc;

namespace KanBan.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly TaskService _taskService;

        public TaskController(TaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var result = await _taskService.GetAllTasksAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var result = await _taskService.GetTaskByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request)
        {
            var result = await _taskService.CreateTaskAsync(request);
            return result.Success
                ? CreatedAtAction(nameof(GetTaskById), new { id = result.Data.Id }, result)
                : BadRequest(result);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] UpdateTaskStatusRequest request)
        {
            request.TaskId = id;
            var result = await _taskService.UpdateTaskStatusAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id}/assign")]
        public async Task<IActionResult> AssignTask(int id, [FromBody] AssignTaskRequest request)
        {
            request.TaskId = id;
            var result = await _taskService.AssignTaskAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
