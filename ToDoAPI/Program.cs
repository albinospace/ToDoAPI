using Microsoft.EntityFrameworkCore;
using ToDoAPI.Data;
using ToDoAPI.Models;
using ToDoAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IToDoService, ToDoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();

    if (!dbContext.ToDoItems.Any())
    {
        dbContext.ToDoItems.AddRange(
            new ToDoItem
            {
                Title = "Call grandma",
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow.AddDays(-3)
            },
            new ToDoItem
            {
                Title = "Write report",
                Description = "Should be made due Friday",
                IsCompleted = true,
                CreatedAt = DateTime.UtcNow.AddDays(-2),
                CompletedAt = DateTime.UtcNow.AddDays(-1)
            },
            new ToDoItem
            {
                Title = "Update social media posts",
                Description = "Medias: X, Insta, VK",
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            }
        );
        dbContext.SaveChanges();
    }
}

app.Run();
