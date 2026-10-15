//usings---------------------------------------
using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Data;
using TaskManager.Api.Services;
using TaskManager.Api.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// Register_services--------------------------------------------
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Services------------------------------------------
builder.Services.AddScoped<ITaskService, TaskService>();

// PostgreSQL-----------------------------------------------------
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "ConnectionStrings:DefaultConnection is not configured.");
}

builder.Services.AddDbContext<TaskDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

// Build_application------------------------------
var app = builder.Build();
//Swagger----------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Configure_HTTP_request----------------------------
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Map_controller_routes----------------------------
app.MapControllers();

app.Run();
