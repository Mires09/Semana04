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

Funcionario[] listafuncionarios = new Funcionario[100];
int totalFuncionarios = 0;

app.MapGet("/", () =>
{
    return Results.Ok("API SkyMoon funcionando com sucesso!");
});

app.MapPost("/funcionario", (JsonElement body) =>
{
    Random random = new();

    Funcionario funcionario = new Funcionario();

    funcionario.Id = random.Next(1000,9999);
    funcionario.Nome = body.GetProperty("nome").GetString();
    funcionario.Idade = body.GetProperty("idade").GetInt16();
    funcionario.Cargo = body.GetProperty("cargo").GetString();
    funcionario.Departamento = body.GetProperty("departamento").GetString();
    funcionario.Salario = body.GetProperty("salario").GetDouble();

    listafuncionarios[totalFuncionarios] = funcionario;
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