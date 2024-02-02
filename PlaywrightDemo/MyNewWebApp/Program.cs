using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => Results.Ok("Bienvenido a la API de tareas!"));

List<MyTask> tasks = new()
{
    new MyTask(1, "Task 1"),
    new MyTask(2, "Task 2"),
    new MyTask(3, "Task 3")
};

app.MapGet("/tasks", () => Results.Ok(tasks));

app.MapGet("/tasks/{id}", (HttpContext httpContext) => 
{
    var id = int.Parse(httpContext.Request.RouteValues["id"].ToString());
    var task = tasks.FirstOrDefault(t => t.Id == id);
    if (task == null)
    {
        return Results.NotFound($"No task found with ID {id}");
    }
    return Results.Ok(task);
});

// Agregar una task a la lista de tasks, que el nombre de l variable sea newTask no task
app.MapPost("/tasks", (MyTask newTask) => 
{
    tasks.Add(newTask);
    return Results.Created($"/tasks/{newTask.Id}", newTask);
});



app.Run();

public record MyTask(int Id, string Name);