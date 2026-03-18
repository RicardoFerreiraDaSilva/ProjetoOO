using PizzariaApp.Models;
using PizzariaFramework.Data;
using Microsoft.Data.Sqlite;

// --- Configuração Inicial do Banco (O Framework deveria automatizar isso, mas vamos manter simples agora) ---
using (var conn = DatabaseConfig.GetConnection())
{
    var cmd = conn.CreateCommand();
    cmd.CommandText = "CREATE TABLE IF NOT EXISTS Pizza (Id INTEGER PRIMARY KEY AUTOINCREMENT, Nome TEXT, Preco DECIMAL, Tamanho TEXT)";
    cmd.ExecuteNonQuery();
}

var pizzaDao = new BaseDAO<Pizza>();
bool executando = true;

Console.WriteLine("=== SISTEMA DE PIZZARIA (FRAMEWORK XYZ) ===");

while (executando)
{
    Console.WriteLine("\nEscolha uma opção:");
    Console.WriteLine("1. Cadastrar Pizza");
    Console.WriteLine("2. Listar Cardápio");
    Console.WriteLine("0. Sair");
    Console.Write("Opção: ");

    string opcao = Console.ReadLine() ?? "";

    switch (opcao)
    {
        case "1":
            Console.Write("Nome da Pizza: ");
            string nome = Console.ReadLine() ?? "";
            
            Console.Write("Preço: ");
            decimal preco = decimal.Parse(Console.ReadLine() ?? "0");
            
            Console.Write("Tamanho (P/M/G): ");
            string tamanho = Console.ReadLine() ?? "";

            var novaPizza = new Pizza(nome, preco, tamanho);
            pizzaDao.Inserir(novaPizza);
            
            Console.WriteLine("✅ Pizza salva com sucesso via Framework!");
            break;

        case "2":
            Console.WriteLine("\n--- CARDÁPIO ATUAL ---");
            var lista = pizzaDao.ListarTodos();
            if (lista.Count == 0) Console.WriteLine("Nenhuma pizza cadastrada.");
            foreach (var p in lista)
            {
                Console.WriteLine(p);
            }
            break;

        case "0":
            executando = false;
            break;

        default:
            Console.WriteLine("Opção inválida!");
            break;
    }
}

Console.WriteLine("Encerrando sistema...");