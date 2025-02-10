using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using tasks_api.Classes;
using tasks_api.Context;

[Route("api/[controller]")]
[ApiController]
public class CustomTasksController : ControllerBase
{
    private readonly CustomTaskDbContext _dbContext;

    public CustomTasksController(CustomTaskDbContext context)
    {
        _dbContext = context;
    }

    // GET: api/customtasks
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomTask>>> GetTasks()
    {
        return await _dbContext.CustomTasks.ToListAsync();
    }

    // GET: api/customtasks/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<CustomTask>> GetTask(int id)
    {
        var task = await _dbContext.CustomTasks.FindAsync(id);
        if (task == null)
        {
            return NotFound();
        }
        return task;
    }

    // POST: api/customtasks
    [HttpPost]
    public async Task<ActionResult<CustomTask>> CreateTask(CustomTask task)
    {
        _dbContext.CustomTasks.Add(task);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
    }

    // PUT: api/customtasks/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, CustomTask updatedTask)
    {
        if (id != updatedTask.Id)
        {
            return BadRequest();
        }

        _dbContext.Entry(updatedTask).State = EntityState.Modified;

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_dbContext.CustomTasks.Any(t => t.Id == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/customtasks/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var task = await _dbContext.CustomTasks.FindAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        _dbContext.CustomTasks.Remove(task);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

}
