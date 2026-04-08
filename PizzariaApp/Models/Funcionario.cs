using PizzariaFramework.Models;

namespace PizzariaApp.Models;

public class Funcionario : EntidadeBase
{
    public string Nome { get; set; } = "";
    public string Cargo { get; set; } = "";

    public Funcionario() { }
    public Funcionario(string nome, string cargo)
    {
        Nome = nome;
        Cargo = cargo;
    }

    public override string ToString() => $"[{Id}] Func.: {Nome} - Cargo: {Cargo}";
}