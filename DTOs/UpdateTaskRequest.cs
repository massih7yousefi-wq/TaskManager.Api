using System.ComponentModel.DataAnnotations;
using TaskManager.Api.Models;

namespace TaskManager.Api.DTOs
{
    public class UpdateTaskRequest
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, MinimumLength = 1,
            ErrorMessage = "Title must be between 1 and 200 characters.")]
        public string Title { get; set; } = string.Empty;

        [StringLength(2000,
            ErrorMessage = "Description cannot exceed 2000 characters.")]
        public string? Description { get; set; }

        [EnumDataType(typeof(Priority),
            ErrorMessage = "Invalid priority value.")]
        public Priority Priority { get; set; }

        [EnumDataType(typeof(TaskState),
            ErrorMessage = "Invalid task state.")]
        public TaskState State { get; set; }

        public DateTime? DueDate { get; set; }
    }
}
