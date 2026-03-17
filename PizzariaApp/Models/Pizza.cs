namespace PizzariaApp.Models;

public class Pizza
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public string Tamanho { get; set; } = "Média";

    // Construtor vazio é importante para frameworks de banco de dados
    public Pizza() { }

    public Pizza(string nome, decimal preco, string tamanho)
    {
        Nome = nome;
        Preco = preco;
        Tamanho = tamanho;
    }

    // Sobrescrever o ToString ajuda na hora de listar no Console
    public override string ToString()
    {
        return $"[{Id}] {Nome} ({Tamanho}) - R$ {Preco:F2}";
    }
}