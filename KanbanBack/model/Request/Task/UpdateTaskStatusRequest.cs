namespace ApprendreDotNet.model.Request.Task
{
    public class UpdateTaskStatusRequest
    {
        public int TaskId { get; set; }
        public string NewStatus { get; set; }
        public int ChangedByUserId { get; set; }
    }
}
