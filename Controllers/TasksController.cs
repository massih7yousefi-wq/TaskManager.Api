//usings-------------------------
using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.DTOs;
using TaskManager.Api.Services;
using TaskManager.Api.Services.Interface;


namespace TaskManager.Api.Controllers
{
    //Controller--------------------------------
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        //Constructor-------------------------------
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }
        //Create-----------------------------------------
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateTaskRequest request)
        {
            try
            {
                var task = await _taskService.CreateAsync(request);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = task.Id },
                    task);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        //GetByid------------------------------------
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var task = await _taskService.GetByIdAsync(id);

            if (task is null)
            {
                return NotFound();
            }

            return Ok(task);
        }
        //GetAll------------------------------------
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] TaskQueryParameters parameters)
        {
            var tasks = await _taskService.GetAllAsync(parameters);
            return Ok(tasks);
        }
        //Update-------------------------------------
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id,
               UpdateTaskRequest request)
        {
            try
            {
                var task = await _taskService.UpdateAsync(id, request);

                if (task is null)
                {
                    return NotFound();
                }

                return Ok(task);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        //Delete--------------------------------------------
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _taskService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
        

