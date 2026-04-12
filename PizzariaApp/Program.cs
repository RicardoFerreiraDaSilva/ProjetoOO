using PizzariaApp.Models;
using GenericFramework.Data;
using GenericFramework.Interfaces;
using GenericFramework.Models;
using Microsoft.Data.Sqlite;

// 1. SETUP DO BANCO (Cria as tabelas se não existirem)
using (var conn = DatabaseConfig.GetConnection())
{
    var cmd = conn.CreateCommand();
    cmd.CommandText = "CREATE TABLE IF NOT EXISTS Pizza (Id INTEGER PRIMARY KEY AUTOINCREMENT, Nome TEXT, Preco DECIMAL, Tamanho TEXT)";
    cmd.ExecuteNonQuery();
    
    cmd.CommandText = "CREATE TABLE IF NOT EXISTS Bebida (Id INTEGER PRIMARY KEY AUTOINCREMENT, Nome TEXT, Preco DECIMAL, Litragem TEXT)";
    cmd.ExecuteNonQuery();

    cmd.CommandText = "CREATE TABLE IF NOT EXISTS Sobremesa (Id INTEGER PRIMARY KEY AUTOINCREMENT, Nome TEXT, Preco DECIMAL, Vegana BOOLEAN)";
    cmd.ExecuteNonQuery();

    cmd.CommandText = "CREATE TABLE IF NOT EXISTS Funcionario (Id INTEGER PRIMARY KEY AUTOINCREMENT, Nome TEXT, Cargo TEXT)";
    cmd.ExecuteNonQuery();
}

// 2. REGISTRO DE DAOS
var daos = new Dictionary<string, object>
{
    { "Pizza", new BaseDAO<Pizza>() },
    { "Bebida", new BaseDAO<Bebida>() },
    { "Sobremesa", new BaseDAO<Sobremesa>() },
    { "Funcionario", new BaseDAO<Funcionario>() }
};

bool executando = true;
Console.WriteLine("=== SISTEMA GESTOR (FRAMEWORK) ===");

while (executando)
{
    Console.WriteLine("\nEm qual categoria você deseja mexer?");
    var chaves = daos.Keys.ToList();
    for (int i = 0; i < chaves.Count; i++)
    {
        Console.WriteLine($"{i + 1}. Gerenciar {chaves[i]}");
    }
    Console.WriteLine("0. Sair");
    Console.Write("Opção: ");

    if (!int.TryParse(Console.ReadLine(), out int escolha) || escolha == 0) break;
    if (escolha > chaves.Count) { Console.WriteLine("Opção inválida!"); continue; }

    string categoriaSelecionada = chaves[escolha - 1];
    dynamic daoSelecionado = daos[categoriaSelecionada];

    MenuOperacoes(categoriaSelecionada, daoSelecionado);
}

// 3. MENU DE OPERAÇÕES GENÉRICO
void MenuOperacoes(string nome, dynamic dao)
{
    while (true)
    {
        Console.WriteLine($"\n--- Menu: {nome} ---");
        Console.WriteLine("1. Listar Todos");
        Console.WriteLine("2. Cadastrar Novo");
        Console.WriteLine("3. Remover por ID");
        Console.WriteLine("0. Voltar");
        Console.Write("Escolha: ");
        
        string op = Console.ReadLine() ?? "";
        if (op == "0") break;

        if (op == "1")
        {
            var lista = dao.ListarTodos();
            if (lista.Count == 0) Console.WriteLine("Nenhum registro encontrado.");
            foreach (var item in lista) Console.WriteLine(item);
        }
        else if (op == "2") // Cadastro Genérico
{
    // 1. Descobrimos qual é o tipo de classe que o DAO está usando
    // (ex: Se for BaseDAO<Pizza>, o tipo será Pizza)
    Type tipoDoObjeto = dao.GetType().GetGenericArguments()[0];

    // 2. O Framework faz as perguntas e cria o objeto sozinho
    var novoObjeto = MenuEngine.PreencherObjetoDinamico(tipoDoObjeto);

    // 3. O DAO salva no banco
    dao.Inserir((dynamic)novoObjeto);

    Console.WriteLine($"\n✅ {nome} cadastrado com sucesso via Framework!");
}
}
}