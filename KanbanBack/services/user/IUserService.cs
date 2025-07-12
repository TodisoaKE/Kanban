using ApprendreDotNet.model.Request.User;
using ApprendreDotNet.model.Response;
using ApprendreDotNet.model.Response.user;

namespace ApprendreDotNet.services.user
{
    public interface IUserService
    {
        Task<ResponseModel<UserResponse>> CreateUserAsync(UserRequest request);
        Task<ResponseModel<UserResponse>> EditUserAsync(int id, UserRequest request);
        Task<ResponseModel<UserResponse>> GetUserByIdAsync(int id);
        Task<ResponseModel<List<UserResponse>>> GetAllUsersAsync();
        Task<ResponseModel<bool>> DeleteUserByIdAsync(int id);
    }
}
