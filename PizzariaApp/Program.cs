using PizzariaApp.Models;
using PizzariaFramework.Data;
using PizzariaFramework.Interfaces;
using PizzariaFramework.Models;
using Microsoft.Data.Sqlite; // AJUSTE 1: Necessário para o comando de criar tabela

// --- AJUSTE 2: Garantir que a tabela de Sobremesa exista no SQLite ---
using (var conn = DatabaseConfig.GetConnection())
{
    var cmd = conn.CreateCommand();
    cmd.CommandText = "CREATE TABLE IF NOT EXISTS Pizza (Id INTEGER PRIMARY KEY AUTOINCREMENT, Nome TEXT, Preco DECIMAL, Tamanho TEXT)";
    cmd.ExecuteNonQuery();
    
    cmd.CommandText = "CREATE TABLE IF NOT EXISTS Bebida (Id INTEGER PRIMARY KEY AUTOINCREMENT, Nome TEXT, Preco DECIMAL, Litragem TEXT)";
    cmd.ExecuteNonQuery();

    // Nova tabela para Sobremesa
    cmd.CommandText = "CREATE TABLE IF NOT EXISTS Sobremesa (Id INTEGER PRIMARY KEY AUTOINCREMENT, Nome TEXT, Preco DECIMAL, Vegana BOOLEAN)";
    cmd.ExecuteNonQuery();
}

// 1. Registro de DAOs
var daos = new Dictionary<string, object>
{
    { "Pizza", new BaseDAO<Pizza>() },
    { "Bebida", new BaseDAO<Bebida>() },
    { "Sobremesa", new BaseDAO<Sobremesa>() } 
};

bool executando = true;
Console.WriteLine("=== SISTEMA GESTOR (FRAMEWORK XYZ) ===");

while (executando)
{
    Console.WriteLine("\nEm qual categoria você deseja mexer?");
    int i = 1;
    var chaves = daos.Keys.ToList();
    foreach (var nome in chaves)
    {
        Console.WriteLine($"{i++}. Gerenciar {nome}");
    }
    Console.WriteLine("0. Sair");
    Console.Write("Opção: ");

    if (!int.TryParse(Console.ReadLine(), out int escolha) || escolha == 0) break;

    string categoriaSelecionada = chaves[escolha - 1];
    dynamic daoSelecionado = daos[categoriaSelecionada];

    MenuOperacoes(categoriaSelecionada, daoSelecionado);
}

// 2. Menu de Operações Genérico
void MenuOperacoes(string nome, dynamic dao)
{
    while (true) // Adicionado loop para não sair do menu após uma ação
    {
        Console.WriteLine($"\n--- Menu: {nome} ---");
        Console.WriteLine("1. Listar Todos");
        Console.WriteLine("2. Cadastrar Novo"); // AJUSTE 3: Adicionada opção de cadastro
        Console.WriteLine("3. Remover por ID");
        Console.WriteLine("0. Voltar");
        Console.Write("Escolha: ");
        
        string op = Console.ReadLine() ?? "";

        if (op == "0") break;

        if (op == "1")
        {
            var lista = dao.ListarTodos();
            foreach (var item in lista) Console.WriteLine(item);
        }
        else if (op == "2") // Lógica de cadastro para cada tipo
        {
            Console.Write("Nome: "); string n = Console.ReadLine() ?? "";
            Console.Write("Preço: "); decimal p = decimal.Parse(Console.ReadLine() ?? "0");

            if (nome == "Pizza") {
                Console.Write("Tamanho: "); string t = Console.ReadLine() ?? "";
                dao.Inserir(new Pizza(n, p, t));
            } 
            else if (nome == "Bebida") {
                Console.Write("Litragem: "); string l = Console.ReadLine() ?? "";
                dao.Inserir(new Bebida(n, p, l));
            }
            else if (nome == "Sobremesa") {
                Console.Write("É Vegana? (S/N): "); 
                bool v = (Console.ReadLine() ?? "").ToUpper() == "S";
                dao.Inserir(new Sobremesa(n, p, v));
            }
            Console.WriteLine("✅ Cadastrado com sucesso!");
        }
        else if (op == "3")
        {
            Console.Write("Digite o ID para remover: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                dao.Excluir(id);
                Console.WriteLine("🗑️ Removido!");
            }
        }
    }
}