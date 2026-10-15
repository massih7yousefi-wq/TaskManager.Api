using TaskManager.Api.Models;
using TaskManager.Api.DTOs;


namespace TaskManager.Api.Services.Interface
{
    public interface ITaskService
    {
        Task<TaskItem> CreateAsync (CreateTaskRequest request);

        Task<TaskItem?> GetByIdAsync(Guid id);

        Task<List<TaskItem>> GetAllAsync(TaskQueryParameters parameters);

        Task<TaskItem?> UpdateAsync(Guid id,
             UpdateTaskRequest request);

        Task<bool> DeleteAsync(Guid id);
    }
}
