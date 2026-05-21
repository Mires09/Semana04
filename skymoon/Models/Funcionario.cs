namespace skymoon.Models;

public class Funcionario {
    
    // atributos
    private int id;
    private string? nome;
    private int idade;
    private string? cargo;
    private string? departamento;
    private double salario;

    public int Id
    {
        get { return id; }
        set { id = value; }
    }
    public string? Nome
    {
        get { return nome; }
        set { nome = value; }
    }
    public int Idade
    {
        get { return idade; }
        set { idade = value; }
    }
    public string? Cargo
    {
        get { return cargo; }
        set { cargo = value; }
    }
    public string? Departamento
    {
        get { return departamento; }
        set { departamento = value; }
    }
    public double Salario
    {
        get { return salario; }
        set { salario = value; }
    }
}