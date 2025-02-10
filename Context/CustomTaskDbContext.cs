using Microsoft.EntityFrameworkCore;
using tasks_api.Classes;

namespace tasks_api.Context
{
    public class CustomTaskDbContext : DbContext
    {
        public DbSet<CustomTask> CustomTasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=customtask.db");
    }
}
