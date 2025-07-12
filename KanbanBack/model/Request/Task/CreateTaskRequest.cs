namespace ApprendreDotNet.model.Request.Task
{
    public class CreateTaskRequest
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public int? AssignedToUserId { get; set; }
        public int CreatedByUserId { get; set; }
    }
}
