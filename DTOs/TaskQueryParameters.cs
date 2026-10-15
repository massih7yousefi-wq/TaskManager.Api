using TaskManager.Api.Models;

namespace TaskManager.Api.DTOs
{
    public class TaskQueryParameters
    {
        public TaskState? State { get; set; }

        public Priority? Priority { get; set; }

        public string? SortBy { get; set; }

        public bool Descending { get; set; } = false;
    }
}