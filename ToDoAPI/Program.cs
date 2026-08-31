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
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IColumnService, ColumnService>();

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
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (!db.Projects.Any())
    {
        var project = new Project
        {
            Title = "Learn C#",
            Description = "Unity to Backend Transition",
            CreatedAt = DateTime.UtcNow.AddDays(-5)
        };
        db.Projects.Add(project);
        db.SaveChanges();

        var colTodo = new Column { Title = "To Do", Order = 1, ProjectId = project.Id, CreatedAt = DateTime.UtcNow };
        var colProgress = new Column { Title = "In Progress", Order = 2, ProjectId = project.Id, CreatedAt = DateTime.UtcNow };
        var colDone = new Column { Title = "Done", Order = 3, ProjectId = project.Id, CreatedAt = DateTime.UtcNow };

        db.Columns.AddRange(colTodo, colProgress, colDone);
        db.SaveChanges();

        db.ToDoItems.AddRange(
            new ToDoItem
            {
                Title = "Learn EF Core",
                Description = "",
                Order = 1,
                ColumnId = colTodo.Id,
                CreatedAt = DateTime.UtcNow.AddDays(-4)
            },
            new ToDoItem
            {
                Title = "Watch Youtube videos on theme",
                Description = "Some video link",
                IsCompleted = true,
                CompletedAt = DateTime.UtcNow.AddDays(-1),
                Order = 1,
                ColumnId = colDone.Id,
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            }
        );
        db.SaveChanges();
    }
}

app.Run();
