using ApprendreDotNet.model.Entities.kanban;

namespace ApprendreDotNet.model.Response.Task
{
    public class TaskWithHistoryResponse : TaskResponse
    {
        public List<TaskHistoryEntity> History { get; set; } = new();
    }
}
