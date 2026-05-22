using skymoon.Models;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.WebHost.UseUrls("http://0.0.0.0:8000");

var app = builder.Build();

app.UseCors("AllowAll");

Funcionario[] funcionarios = new Funcionario[100];
int totalFuncionarios = 0;

app.MapGet("/", () =>
{
    return Results.Ok("API SkyMoon funcionando com sucesso!");
});

app.MapPost("/funcionario", (JsonElement body) =>
{
    Funcionario funcionario = new Funcionario();

    funcionario.Nome = body.GetProperty("nome").GetString();

    Console.WriteLine(funcionario.Nome);

    funcionarios[totalFuncionarios] = funcionario;
    totalFuncionarios++;

    return Results.Ok(
        new{
            funcionario
        }
    );
});

/* 
app.MapGet("/funcionario", () =>
{
    
});

app.MapPatch("/funcionario/{id}", (int id, JsonElement body) =>
{
    
});

app.MapPut("/funcionario/{id}", (int id, JsonElement body) =>
{   
    
});

app.MapDelete("/funcionario", (int id) =>
{
    
});

app.MapGet("/funcionario/departamento/busca", (string departamento) =>
{
    
});

app.MapGet("/funcionario/busca", (string nome) =>
{
   
}); */

app.Run();