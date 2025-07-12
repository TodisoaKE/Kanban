namespace ApprendreDotNet.model.Request.Task
{
    public class AssignTaskRequest
    {
        public int TaskId { get; set; }
        public int? AssignedToUserId { get; set; }
        public int ChangedByUserId { get; set; }
    }
}
