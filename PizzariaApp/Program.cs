using PizzariaApp.Models;
using PizzariaFramework.Data;
using PizzariaFramework.Interfaces;
using PizzariaFramework.Models;

// 1. Registro de DAOs (O ÚNICO lugar onde você mexe para adicionar novas classes)
var daos = new Dictionary<string, object>
{
    { "Pizza", new BaseDAO<Pizza>() },
    { "Bebida", new BaseDAO<Bebida>() }
    // Para adicionar Sobremesa, seria apenas: { "Sobremesa", new BaseDAO<Sobremesa>() }
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

    // Seleciona o nome da classe escolhida (ex: "Pizza")
    string categoriaSelecionada = chaves[escolha - 1];
    dynamic daoSelecionado = daos[categoriaSelecionada]; // O 'dynamic' permite chamar métodos do DAO sem saber o tipo T agora

    MenuOperacoes(categoriaSelecionada, daoSelecionado);
}

// 2. Menu de Operações Genérico (Funciona para QUALQUER classe)
void MenuOperacoes(string nome, dynamic dao)
{
    Console.WriteLine($"\n--- Menu: {nome} ---");
    Console.WriteLine("1. Listar Todos");
    Console.WriteLine("2. Remover por ID");
    Console.WriteLine("0. Voltar");
    Console.Write("Escolha: ");
    
    string op = Console.ReadLine() ?? "";

    if (op == "1")
    {
        var lista = dao.ListarTodos();
        foreach (var item in lista) Console.WriteLine(item);
    }
    else if (op == "2")
    {
        Console.Write("Digite o ID para remover: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            dao.Excluir(id);
            Console.WriteLine("🗑️ Removido!");
        }
    }
}