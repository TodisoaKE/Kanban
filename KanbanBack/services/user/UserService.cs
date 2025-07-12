using ApprendreDotNet.DbCore;
using ApprendreDotNet.model.Entities.user;
using ApprendreDotNet.model.Request.User;
using ApprendreDotNet.model.Response;
using ApprendreDotNet.model.Response.user;
using Microsoft.EntityFrameworkCore;

namespace ApprendreDotNet.services.user
{
    public class UserService : IUserService
    {
        private readonly MyAppDbContext _db;
        private readonly ILogger<UserService> _logger;

        public UserService(MyAppDbContext db, ILogger<UserService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<ResponseModel<UserResponse>> CreateUserAsync(UserRequest request)
        {
            var user = new UserEntity
            {
                Name = request.Name,
                Email = request.Email,
                CreatedAt = DateTime.UtcNow
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return new ResponseModel<UserResponse>
            {
                Data = MapToResponse(user),
                ResponseCode = "CREATED",
                ResponseMessage = "User created successfully."
            };
        }

        public async Task<ResponseModel<UserResponse>> EditUserAsync(int id, UserRequest request)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null)
                return NotFoundResponse<UserResponse>("User not found");

            user.Name = request.Name;
            user.Email = request.Email;

            await _db.SaveChangesAsync();

            return new ResponseModel<UserResponse>
            {
                Data = MapToResponse(user),
                ResponseCode = "UPDATED",
                ResponseMessage = "User updated successfully."
            };
        }

        public async Task<ResponseModel<UserResponse>> GetUserByIdAsync(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null)
                return NotFoundResponse<UserResponse>("User not found");

            return new ResponseModel<UserResponse>
            {
                Data = MapToResponse(user),
                ResponseCode = "FOUND",
                ResponseMessage = "User found."
            };
        }

        public async Task<ResponseModel<List<UserResponse>>> GetAllUsersAsync()
        {
            var users = await _db.Users.ToListAsync();
            return new ResponseModel<List<UserResponse>>
            {
                Data = users.Select(MapToResponse).ToList(),
                ResponseCode = "LISTED",
                ResponseMessage = "Users retrieved successfully."
            };
        }

        public async Task<ResponseModel<bool>> DeleteUserByIdAsync(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null)
                return NotFoundResponse<bool>("User not found");

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();

            return new ResponseModel<bool>
            {
                Data = true,
                ResponseCode = "DELETED",
                ResponseMessage = "User deleted."
            };
        }

        private static UserResponse MapToResponse(UserEntity entity) => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Email = entity.Email,
            CreatedAt = entity.CreatedAt.ToString("o")
        };

        private static ResponseModel<T> NotFoundResponse<T>(string msg) => new()
        {
            Success = false,
            ResponseCode = "NOT_FOUND",
            ResponseMessage = msg,
            Errors = new List<ErrorModel>
            {
                new() { ErrorCode = "404", ErrorMessage = msg }
            }
        };
    }
}
