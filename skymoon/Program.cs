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

// builder.WebHost.UseUrls("http://0.0.0.0:8000");

var app = builder.Build();

app.UseCors("AllowAll");


Funcionario[] listafuncionarios = new Funcionario[100];
int totalFuncionarios = 0;

app.MapGet("/", () =>
{
    return Results.Ok("API SkyMoon funcionando com sucesso!");
});

//Adicionar novos funcionários
app.MapPost("/cadastrarfuncionarios", (JsonElement body) =>
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

//Listar os funcionários cadastrados
app.MapGet("/listarfuncionarios", () =>
{
    Funcionario[] funcionariosCadastrados = new Funcionario[totalFuncionarios];

    for (int i = 0; i < totalFuncionarios; i++)
    {
        funcionariosCadastrados[i] = listafuncionarios[i];
    }

    return Results.Ok(new
    {
        funcionariosCadastrados
    });
});


//Deletar funcionários
app.MapDelete("/deletarfuncionario/{id}", (int id) =>
{
    int index = -1;

    // Procurar funcionário pelo Id
    for (int i = 0; i < totalFuncionarios; i++)
    {
        if (listafuncionarios[i].Id == id)
        {
            index = i;
            break;
        }
    }

    if (index == -1)
    {
        return Results.NotFound(new { 
            mensagem = "Funcionário não encontrado." 
    });
    }

    // Remover
    for (int i = index; i < totalFuncionarios - 1; i++)
    {
        listafuncionarios[i] = listafuncionarios[i + 1];
    }

    totalFuncionarios--;

    return Results.Ok(new { 
        mensagem = "Funcionário removido com sucesso." 
        });
});

//Atualizar funcionário
app.MapPut("/atualizarfuncionario/{id}", (int id, JsonElement body) =>
{
    Funcionario? funcionario = null;

    // Procurar funcionário pelo Id
    for (int i = 0; i < totalFuncionarios; i++)
    {
        if (listafuncionarios[i].Id == id)
        {
            funcionario = listafuncionarios[i];

            funcionario.Nome = body.GetProperty("nome").GetString();
            funcionario.Idade = body.GetProperty("idade").GetInt16();
            funcionario.Cargo = body.GetProperty("cargo").GetString();
            funcionario.Departamento = body.GetProperty("departamento").GetString();
            funcionario.Salario = body.GetProperty("salario").GetDouble();

            listafuncionarios[i] = funcionario;
            break;
        }
    }

    if (funcionario == null)
    {
        return Results.NotFound(new { 
            mensagem = "Funcionário não encontrado." 
            });
    }

    return Results.Ok(new { 
        mensagem = "Funcionário atualizado com sucesso.", funcionario 
        });
});


//Atualizar parcialmente funcionário
app.MapPatch("/atualizarfuncionario/{id}", (int id, JsonElement body) =>
{
    Funcionario? funcionario = null;

    // Procurar funcionário pelo Id
    for (int i = 0; i < totalFuncionarios; i++)
    {
        if (listafuncionarios[i].Id == id)
        {
            funcionario = listafuncionarios[i];

            if (body.TryGetProperty("nome", out var nome))
                funcionario.Nome = nome.GetString();

            if (body.TryGetProperty("idade", out var idade))
                funcionario.Idade = idade.GetInt16();

            if (body.TryGetProperty("cargo", out var cargo))
                funcionario.Cargo = cargo.GetString();

            if (body.TryGetProperty("departamento", out var departamento))
                funcionario.Departamento = departamento.GetString();

            if (body.TryGetProperty("salario", out var salario))
                funcionario.Salario = salario.GetDouble();

            listafuncionarios[i] = funcionario;
            break;
        }
    }

if (funcionario == null)
    {
        return Results.NotFound(new 
        { 
            mensagem = "Funcionário não encontrado!" 
            });
    }

    return Results.Ok(new 
    { 
        mensagem = "Funcionário atualizado parcialmente com sucesso.", 
        funcionario 
        });

});


// Buscar funcionários
app.MapGet("/funcionario/departamento/busca", (string departamento) =>
{
    Funcionario[] funcionariosEncontrados = new Funcionario[totalFuncionarios];

    int totalEncontrados = 0;

    for (int i = 0; i < totalFuncionarios; i++)
    {
        if (listafuncionarios[i].Departamento?.ToLower() == departamento.ToLower())
        {
            funcionariosEncontrados[totalEncontrados] = listafuncionarios[i];
            totalEncontrados++;
        }
    }

    if (totalEncontrados > 0)
    {
        Funcionario[] resultadoFinal = new Funcionario[totalEncontrados];

        for (int i = 0; i < totalEncontrados; i++)
        {
            resultadoFinal[i] = funcionariosEncontrados[i];
        }        

        return Results.Ok(new
        {
            departamento,
            funcionarios = resultadoFinal
        });
    } 

    return Results.NotFound(new
    {
        message = "Nenhum funcionário encontrado para esse departamento."
    });
});

app.MapGet("/funcionario/busca", (string nome) =>
{
    Funcionario[] funcionariosEncontrados = new Funcionario[totalFuncionarios];

    int totalEncontrados = 0;

    for (int i = 0; i < totalFuncionarios; i++)
    {
        if (listafuncionarios[i].Nome?.ToLower() == nome.ToLower())
        {
            funcionariosEncontrados[totalEncontrados] = listafuncionarios[i];
            totalEncontrados++;
        }
    }

    if (totalEncontrados > 0)
    {
        Funcionario[] resultadoFinal = new Funcionario[totalEncontrados];

        for (int i = 0; i < totalEncontrados; i++)
        {
            resultadoFinal[i] = funcionariosEncontrados[i];
        }        

        return Results.Ok(new
        {
            nome,
            funcionarios = resultadoFinal
        });
    } 

    return Results.NotFound(new
    {
        message = "Nenhum funcionário encontrado esse nome."
    });
});

app.Run();