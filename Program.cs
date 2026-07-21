using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Todo.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var connectionstr = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<TodoContext>(op => op.UseSqlServer(connectionstr));
builder.Services.AddDbContext<ApplicationDbContext>(op => op.UseSqlServer(connectionstr));

builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapIdentityApi<IdentityUser>();

app.MapControllers();

app.Run();
