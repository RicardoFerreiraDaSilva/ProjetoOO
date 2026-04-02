using PizzariaApp.Models;
using PizzariaFramework.Data;
using Microsoft.Data.Sqlite;

// 1. Setup do Banco (Garantindo que as tabelas existam para o framework usar)
using (var conn = DatabaseConfig.GetConnection())
{
    var cmd = conn.CreateCommand();
    // Tabela de Pizzas
    cmd.CommandText = "CREATE TABLE IF NOT EXISTS Pizza (Id INTEGER PRIMARY KEY AUTOINCREMENT, Nome TEXT, Preco DECIMAL, Tamanho TEXT)";
    cmd.ExecuteNonQuery();
    // Tabela de Bebidas (O NOME DA TABELA DEVE SER IGUAL AO DA CLASSE)
    cmd.CommandText = "CREATE TABLE IF NOT EXISTS Bebida (Id INTEGER PRIMARY KEY AUTOINCREMENT, Nome TEXT, Preco DECIMAL, Litragem TEXT)";
    cmd.ExecuteNonQuery();
}

// 2. Instanciando os DAOs (O mesmo motor para modelos diferentes!)
var pizzaDao = new BaseDAO<Pizza>();
var bebidaDao = new BaseDAO<Bebida>();

bool executando = true;
Console.WriteLine("=== SISTEMA DE PIZZARIA (FRAMEWORK XYZ) ===");

while (executando)
{
    Console.WriteLine("\nEscolha uma opção:");
    Console.WriteLine("1. Cadastrar Pizza");
    Console.WriteLine("2. Listar Cardápio de Pizzas");
    Console.WriteLine("3. Cadastrar Bebida");
    Console.WriteLine("4. Listar Bebidas");
    Console.WriteLine("5. Remover Pizza (por ID)"); 
    Console.WriteLine("6. Remover Bebida (por ID)");
    Console.WriteLine("0. Sair");
    Console.Write("Opção: ");

    string opcao = Console.ReadLine() ?? "";

    switch (opcao)
    {
        case "1":
            // ... (seu código de cadastrar pizza continua igual)
            Console.Write("Nome da Pizza: ");
            string nomeP = Console.ReadLine() ?? "";
            Console.Write("Preço: ");
            decimal precoP = decimal.Parse(Console.ReadLine() ?? "0");
            Console.Write("Tamanho: ");
            string tamP = Console.ReadLine() ?? "";
            pizzaDao.Inserir(new Pizza(nomeP, precoP, tamP));
            Console.WriteLine("✅ Pizza salva!");
            break;

        case "2":
            Console.WriteLine("\n--- PIZZAS ---");
            pizzaDao.ListarTodos().ForEach(p => Console.WriteLine(p));
            break;

        case "3": // NOVA OPÇÃO
            Console.Write("Nome da Bebida: ");
            string nomeB = Console.ReadLine() ?? "";
            Console.Write("Preço: ");
            decimal precoB = decimal.Parse(Console.ReadLine() ?? "0");
            Console.Write("Litragem (ex: 2L, 350ml): ");
            string litros = Console.ReadLine() ?? "";
            
            // Usando o DAO de Bebida!
            bebidaDao.Inserir(new Bebida(nomeB, precoB, litros));
            Console.WriteLine("✅ Bebida salva!");
            break;

        case "4": // NOVA OPÇÃO
            Console.WriteLine("\n--- BEBIDAS ---");
            bebidaDao.ListarTodos().ForEach(b => Console.WriteLine(b));
            break;

        case "5": // OPÇÃO: REMOVER PIZZA
            Console.Write("Digite o ID da Pizza que deseja remover: ");
            if (int.TryParse(Console.ReadLine(), out int idPizza))
            {
                pizzaDao.Excluir(idPizza);
                Console.WriteLine("🗑️ Pizza removida com sucesso!");
            }
            break;

        case "6": // OPÇÃO: REMOVER BEBIDA
            Console.Write("Digite o ID da Bebida que deseja remover: ");
            if (int.TryParse(Console.ReadLine(), out int idBebida))
            {
                bebidaDao.Excluir(idBebida);
                Console.WriteLine("🗑️ Bebida removida com sucesso!");
            }
            break;
        case "0":
            executando = false;
            break;
    }
}