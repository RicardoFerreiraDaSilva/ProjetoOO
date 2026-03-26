using PizzariaFramework.Models;

namespace PizzariaApp.Models;

public class Bebida : EntidadeBase
{
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public string Litragem { get; set; } = "";

    public Bebida() { }
    
    public Bebida(string nome, decimal preco, string litragem)
    {
        Nome = nome;
        Preco = preco;
        Litragem = litragem;
    }

    public override string ToString() => $"[{Id}] Bebida: {Nome} ({Litragem}) - R$ {Preco:F2}";
}