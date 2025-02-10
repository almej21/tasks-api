using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using SQLitePCL;
using System;
using System.Xml.Linq;
using tasks_api.Classes;
using tasks_api.Context;
using tasks_api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5001");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // React frontend
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

// Configure SQLite Database
builder.Services.AddDbContext<CustomTaskDbContext>();

builder.Services.AddHostedService(provider =>
{
    // Resolve the background service from the provider
    var scope = provider.CreateScope();  // Create a scope for the background service
    var dbContext = scope.ServiceProvider.GetRequiredService<CustomTaskDbContext>();
    return new CustomTaskStatusUpdateService(dbContext, provider.GetRequiredService<ILogger<CustomTaskStatusUpdateService>>());
});


builder.Services.AddControllers();

var app = builder.Build();

app.UseCors("AllowReactApp");

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();

