using ApprendreDotNet.model.Request.User;
using ApprendreDotNet.model.Response.user;
using ApprendreDotNet.model.Response;

namespace KanBan.services.task
{
    public interface ITaskService
    {
        Task<ResponseModel<UserResponse>> CreateTaskAsync(UserRequest request);
        Task<ResponseModel<UserResponse>> EditTaskAsync(int id, UserRequest request);
        Task<ResponseModel<UserResponse>> GetTaskByIdAsync(int id);
        Task<ResponseModel<List<UserResponse>>> GetAllTaskAsync();
    }
}
