namespace ApprendreDotNet.model.Response.user
{
    public class UserResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string CreatedAt { get; set; }
    }
}
