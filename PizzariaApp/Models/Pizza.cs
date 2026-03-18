using PizzariaFramework.Models; 

namespace PizzariaApp.Models;

public class Pizza : EntidadeBase
{
    
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public string Tamanho { get; set; } = "Média";

    public Pizza() { }

    public Pizza(string nome, decimal preco, string tamanho)
    {
        Nome = nome;
        Preco = preco;
        Tamanho = tamanho;
    }

    public override string ToString()
    {
        // O Id continua funcionando aqui porque foi herdado!
        return $"[{Id}] {Nome} ({Tamanho}) - R$ {Preco:F2}";
    }
}