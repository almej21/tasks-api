using System;
using tasks_api.Context;
using Microsoft.EntityFrameworkCore;
using tasks_api.Classes;

namespace tasks_api.Services
{
    public class CustomTaskStatusUpdateService : BackgroundService
    {
        private readonly CustomTaskDbContext _dbContext;
        private readonly ILogger<CustomTaskStatusUpdateService> _logger;

        public CustomTaskStatusUpdateService(CustomTaskDbContext dbContext, ILogger<CustomTaskStatusUpdateService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(">>> Background service started...");
            // Run this every day (24 hours)
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken); // wait for 1 day

                try
                {
                    // Get tasks where the due date is passed but they are still Pending or In Progress
                    var overdueTasks = await _dbContext.CustomTasks
                        .Where(t => t.DueDate.HasValue && t.DueDate < DateTime.Now)
                        .ToListAsync();

                    foreach (var task in overdueTasks)
                    {
                        if (task.Status == CustomTaskStatus.Pending)
                        {
                            task.Status = CustomTaskStatus.Overdue;
                        }
                        else if (task.Status == CustomTaskStatus.InProgress)
                        {
                            task.Status = CustomTaskStatus.Completed;
                        }
                    }

                    // Save changes to the database
                    await _dbContext.SaveChangesAsync();
                    _logger.LogInformation("Task statuses updated successfully.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while updating task statuses.");
                }
            }
        }
    }
}
