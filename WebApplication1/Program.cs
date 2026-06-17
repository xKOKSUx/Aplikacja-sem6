using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.Middleware;
using WebApplication1;
using WebApplication1.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Use SQL Server (connection string configured in appsettings). For local development
// you can run SQL Server in Docker (see docker-compose.yml).
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? "Server=localhost,1433;Database=CharacterDb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();

builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddSignalR();

// Swagger for API exploration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Ensure database exists (create database/tables if necessary).
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // For simple scenarios EnsureCreated will create the database if it does not exist.
    db.Database.EnsureCreated();
}

app.UseHttpsRedirection();

// Simple API-key based auth middleware (demo)
app.UseMiddleware<SimpleApiKeyMiddleware>();

app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();
app.MapHub<CharacterHub>("/characterHub");

app.Run();
