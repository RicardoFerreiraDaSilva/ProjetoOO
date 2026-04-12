using GenericFramework.Models;

namespace PizzariaApp.Models;

public class Sobremesa : EntidadeBase
{
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public bool Vegana { get; set; } // Um campo diferente para testar o Framework

    public Sobremesa() { }

    public Sobremesa(string nome, decimal preco, bool vegana)
    {
        Nome = nome;
        Preco = preco;
        Vegana = vegana;
    }

    public override string ToString() 
    {
        string tipo = Vegana ? "🌱 Vegana" : "Tradicional";
        return $"[{Id}] {Nome} ({tipo}) - R$ {Preco:F2}";
    }
}