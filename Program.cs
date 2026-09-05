using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using Todo.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionstr = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<TodoContext>(op => op.UseSqlServer(connectionstr));
builder.Services.AddDbContext<ApplicationDbContext>(op => op.UseSqlServer(connectionstr));

builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapIdentityApi<IdentityUser>();

var gr = app.MapGroup("/Todo");

gr.MapGet("/all", async (TodoContext tdc) =>
{
    return await tdc.Todos.ToListAsync();
});

gr.MapGet("/{id}", async (TodoContext tdc, int id) =>
{
    return await tdc.Todos.FindAsync(id) is Todos todo ? Results.Ok(todo) : Results.NotFound();
});

gr.MapPost("", async (HttpContext context, TodoContext tdc, Todos t) =>
{
    var us = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
    t.UserId = us;
    tdc.Todos.Add(t);
    await tdc.SaveChangesAsync();
    return Results.Ok();
}).RequireAuthorization();

gr.MapDelete("/{id}", async (int id, TodoContext tdc) =>
{
    var v = await tdc.Todos.FindAsync(id);
    if(v is not null)
    {
        tdc.Todos.Remove(v);
        await tdc.SaveChangesAsync();
        return Results.Ok();
    }

    return Results.NotFound();
});

gr.MapPut("/{id}", async (TodoContext tdc, int id, Todos t) =>
{
    var v = await tdc.Todos.FindAsync(id);
    if (v is not null)
    {
        v.Title = t.Title;
        v.Description = t.Description;
        v.UpdatedDate = DateTime.Now;
        await tdc.SaveChangesAsync();
        return Results.Ok();
    }
    return Results.NotFound();
});

app.Run();
