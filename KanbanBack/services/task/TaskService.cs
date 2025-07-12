using ApprendreDotNet.DbCore;
using ApprendreDotNet.model.Entities.kanban;
using ApprendreDotNet.model.Request.Task;
using ApprendreDotNet.model.Response;
using ApprendreDotNet.model.Response.Task;
using KanBan.model.Response.task;
using Microsoft.EntityFrameworkCore;
using static ApprendreDotNet.model.Entities.kanban.TaskHistoryEntity;
using TaskResponse = KanBan.model.Response.task.TaskResponse;
using TaskStatus = ApprendreDotNet.model.Entities.kanban.TaskStatus;

namespace ApprendreDotNet.services.task
{
    public class TaskService
    {
        private readonly MyAppDbContext _db;
        private readonly ILogger<TaskService> _logger;

        public TaskService(MyAppDbContext db, ILogger<TaskService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<ResponseModel<List<TaskResponse>>> GetAllTasksAsync()
        {
            var tasks = await _db.Tasks
                .Include(t => t.AssignedToUser)
                .ToListAsync();

            return new ResponseModel<List<TaskResponse>>
            {
                Data = tasks.Select(MapToResponse).ToList(),
                ResponseCode = "LISTED",
                ResponseMessage = "Tasks retrieved successfully."
            };
        }

        public async Task<ResponseModel<TaskWithHistoryResponse>> GetTaskByIdAsync(int id)
        {
            var task = await _db.Tasks
                .Include(t => t.AssignedToUser)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
                return NotFoundResponse<TaskWithHistoryResponse>("Task not found");

            var history = await _db.TaskHistories
                .Where(h => h.TaskId == id)
                .OrderByDescending(h => h.ChangeDate)
                .ToListAsync();

            var response = MapToDetailedResponse(task);
            response.History = history;

            return new ResponseModel<TaskWithHistoryResponse>
            {
                Data = response,
                ResponseCode = "FOUND",
                ResponseMessage = "Task found."
            };
        }

        public async Task<ResponseModel<TaskResponse>> CreateTaskAsync(CreateTaskRequest request)
        {
            var task = new TaskEntity
            {
                Title = request.Title,
                Description = request.Description,
                Status = TaskStatus.ToDo,
                AssignedToUserId = request.AssignedToUserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.Tasks.Add(task);
            await _db.SaveChangesAsync();

            _db.TaskHistories.Add(new TaskHistoryEntity
            {
                TaskId = task.Id,
                ChangedByUserId = request.CreatedByUserId,
                ChangeType = "CREATION",
                NewValue = "Task Created",
                ChangeDate = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();

            return new ResponseModel<TaskResponse>
            {
                Data = MapToResponse(task),
                ResponseCode = "CREATED",
                ResponseMessage = "Task created."
            };
        }

        public async Task<ResponseModel<bool>> UpdateTaskStatusAsync(UpdateTaskStatusRequest request)
        {
            var task = await _db.Tasks.FindAsync(request.TaskId);
            if (task == null)
                return NotFoundResponse<bool>("Task not found");

            if (!Enum.TryParse<TaskStatus>(request.NewStatus, true, out var newStatus))
                return ErrorResponse<bool>("INVALID_STATUS", $"Invalid status: {request.NewStatus}");

            if (!IsValidStatusTransition(task.Status, newStatus))
                return ErrorResponse<bool>("INVALID_TRANSITION", $"Cannot change from {task.Status} to {newStatus}");

            var oldValue = task.Status.ToString();
            task.Status = newStatus;
            task.UpdatedAt = DateTime.UtcNow;

            _db.TaskHistories.Add(new TaskHistoryEntity
            {
                TaskId = task.Id,
                ChangedByUserId = request.ChangedByUserId,
                ChangeType = "STATUS-CHANGE",
                OldValue = oldValue,
                NewValue = newStatus.ToString(),
                ChangeDate = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();

            return new ResponseModel<bool>
            {
                Data = true,
                ResponseCode = "UPDATED",
                ResponseMessage = "Task status updated."
            };
        }

        public async Task<ResponseModel<bool>> AssignTaskAsync(AssignTaskRequest request)
        {
            var task = await _db.Tasks.FindAsync(request.TaskId);
            if (task == null)
                return NotFoundResponse<bool>("Task not found");

            var oldValue = task.Status.ToString() ?? "null";
            task.AssignedToUserId = request.AssignedToUserId;
            task.UpdatedAt = DateTime.UtcNow;

            _db.TaskHistories.Add(new TaskHistoryEntity
            {
                TaskId = task.Id,
                ChangedByUserId = request.ChangedByUserId,
                ChangeType = "ASSSIGNMENT-CHANGE",
                OldValue = oldValue,
                NewValue = request.AssignedToUserId?.ToString() ?? "null",
                ChangeDate = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();

            return new ResponseModel<bool>
            {
                Data = true,
                ResponseCode = "UPDATED",
                ResponseMessage = "Task assignment updated."
            };
        }

        private static TaskResponse MapToResponse(TaskEntity task) => new()
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status.ToString(),
            AssignedTo = task.AssignedToUser?.Name,
            CreatedAt = task.CreatedAt.ToString("o"),
            UpdatedAt = task.UpdatedAt.ToString("o")
        };

        private static TaskWithHistoryResponse MapToDetailedResponse(TaskEntity task) => new()
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status.ToString(),
            AssignedTo = task.AssignedToUser?.Name,
            CreatedAt = task.CreatedAt.ToString("o"),
            UpdatedAt = task.UpdatedAt.ToString("o")
        };

        private bool IsValidStatusTransition(TaskStatus current, TaskStatus target)
        {
            return current switch
            {
                TaskStatus.ToDo => target == TaskStatus.InProgress,
                TaskStatus.InProgress => target == TaskStatus.Done,
                _ => false
            };
        }

        private static ResponseModel<T> NotFoundResponse<T>(string msg) => new()
        {
            Success = false,
            ResponseCode = "NOT_FOUND",
            ResponseMessage = msg,
            Errors = new List<ErrorModel> { new() { ErrorCode = "404", ErrorMessage = msg } }
        };

        private static ResponseModel<T> ErrorResponse<T>(string code, string msg) => new()
        {
            Success = false,
            ResponseCode = code,
            ResponseMessage = msg,
            Errors = new List<ErrorModel> { new() { ErrorCode = code, ErrorMessage = msg } }
        };
    }
}
