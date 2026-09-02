using Microsoft.EntityFrameworkCore;
using ToDoAPI.Data;
using ToDoAPI.Models;
using ToDoAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Paste the token"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IToDoService, ToDoService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IColumnService, ColumnService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
}).
AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (!db.Users.Any())
    {
        var passwordHasher = new Microsoft.AspNetCore.Identity.PasswordHasher<User>();

        var user = new User
        {
            Email = "test@example.com",
            DisplayName = "Test User",
            PasswordHash = passwordHasher.HashPassword(null, "password123"),
            CreatedAt = DateTime.UtcNow.AddDays(-5)
        };

        db.Users.Add(user);
        db.SaveChanges();

        var project = new Project
        {
            Title = "Learn C#",
            Description = "Unity to Backend Transition",
            CreatedAt = DateTime.UtcNow.AddDays(-5),
            UserId = user.Id
        };

        db.Projects.Add(project);
        db.SaveChanges();

        var colTodo = new Column { 
            Title = "To Do", 
            Order = 1, 
            ProjectId = project.Id, 
            CreatedAt = DateTime.UtcNow 
        };

        var colProgress = new Column { 
            Title = "In Progress", 
            Order = 2, 
            ProjectId = project.Id, 
            CreatedAt = DateTime.UtcNow 
        };

        var colDone = new Column { 
            Title = "Done", 
            Order = 3, 
            ProjectId = project.Id, 
            CreatedAt = DateTime.UtcNow 
        };

        db.Columns.AddRange(colTodo, colProgress, colDone);
        db.SaveChanges();

        db.ToDoItems.AddRange(
            new ToDoItem
            {
                Title = "Learn EF Core",
                Description = "",
                Order = 1,
                ColumnId = colTodo.Id,
                CreatedAt = DateTime.UtcNow.AddDays(-4),
                DueDate = DateTime.UtcNow.AddDays(3),
                Priority = Priority.High
            },
            new ToDoItem
            {
                Title = "Watch Youtube videos on theme",
                Description = "Some video link",
                IsCompleted = true,
                CompletedAt = DateTime.UtcNow.AddDays(-1),
                Order = 1,
                ColumnId = colDone.Id,
                CreatedAt = DateTime.UtcNow.AddDays(-2),
            }
        );
        db.SaveChanges();
    }
}

app.Run();
