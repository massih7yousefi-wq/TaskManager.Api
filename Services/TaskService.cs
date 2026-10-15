//usings----------------------------
using TaskManager.Api.Data;
using TaskManager.Api.DTOs;
using TaskManager.Api.Models;
using TaskManager.Api.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Api.Services
{
    public class TaskService : ITaskService
    {
        //Constructor---------------------------

        private readonly TaskDbContext _dbContext;

        public TaskService(TaskDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        //Create---------------------------------
        public async Task<TaskItem> CreateAsync(
            CreateTaskRequest request)
            {
            //null_title--------------------------------
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                throw new ArgumentException(
                    "Task title is required.",
                    nameof(request.Title));
            }
            //make_object---------------------------------
            var task = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = request.Title.Trim(),
                Description = request.Description,
                Priority = request.Priority,
                State = request.State,
                DueDate = request.DueDate,
                CreatedAt = DateTime.UtcNow
            };
            //Save_To_Database------------------
            _dbContext.Tasks.Add(task);

            await _dbContext.SaveChangesAsync();

            return task;
        }
        //GetById----------------------------------
        public async Task<TaskItem?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Tasks.FindAsync(id);
        }
        //GetAll------------------------------------------
        public async Task<List<TaskItem>> GetAllAsync(
               TaskQueryParameters parameters)
        { 
            var query = _dbContext.Tasks.AsQueryable();

            if (parameters.State.HasValue)
            {
                query = query.Where(
                    task => task.State == parameters.State.Value);
            }

            if (parameters.Priority.HasValue)
            {
                query = query.Where(
                    task => task.Priority == parameters.Priority.Value);
            }

            query = parameters.SortBy?.ToLowerInvariant() switch
            {
                "title" => parameters.Descending
                    ? query.OrderByDescending(task => task.Title)
                    : query.OrderBy(task => task.Title),

                "priority" => parameters.Descending
                    ? query.OrderByDescending(task => task.Priority)
                    : query.OrderBy(task => task.Priority),

                "duedate" => parameters.Descending
                    ? query.OrderByDescending(task => task.DueDate)
                    : query.OrderBy(task => task.DueDate),

                "createdat" => parameters.Descending
                    ? query.OrderByDescending(task => task.CreatedAt)
                    : query.OrderBy(task => task.CreatedAt),

                _ => query.OrderBy(task => task.CreatedAt)
            };

            return await query.ToListAsync();
        }
        //Update---------------------------------------
        public async Task<TaskItem?> UpdateAsync(Guid id,
               UpdateTaskRequest request)
        {
            //1)find_Task----------
            var task = await _dbContext.Tasks.FindAsync(id);

            //not_Found----------------------
            if (task is null)
            {
                return null;
            }
            //null_input---------------------------------
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                throw new ArgumentException(
                    "Task title is required.",
                    nameof(request.Title));
            }
            //Update-------------
            task.Title = request.Title.Trim();
            task.Description = request.Description;
            task.Priority = request.Priority;
            task.State = request.State;
            task.DueDate = request.DueDate;
            task.UpdatedAt = DateTime.UtcNow;
            //Save----------------
            await _dbContext.SaveChangesAsync();

            return task;

        }
        //ِDelete----------------------------------------
        public async Task<bool> DeleteAsync(Guid id)
        {
            var task = await _dbContext.Tasks.FindAsync(id);

            if (task is null)
            {
                return false;
            }

            _dbContext.Tasks.Remove(task);

            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
