using System.Reflection;
using Microsoft.Data.Sqlite;
using PizzariaFramework.Interfaces;
using PizzariaFramework.Models;

namespace PizzariaFramework.Data;

public class BaseDAO<T> : IGenericDAO<T> where T : EntidadeBase, new()
{
    private readonly string _tableName;

    public BaseDAO()
    {
        // Define o nome da tabela como o nome da classe (ex: "Pizza")
        _tableName = typeof(T).Name;
    }

    public void Inserir(T entidade)
    {
        using var connection = DatabaseConfig.GetConnection();
        
        // Pega todas as propriedades da classe (Nome, Preco, etc)
        PropertyInfo[] propriedades = typeof(T).GetProperties();
        
        // Filtra para ignorar o "Id", pois o banco gera automático
        var colunas = propriedades.Where(p => p.Name != "Id").Select(p => p.Name);
        var parametros = colunas.Select(c => "@" + c);

        string sql = $"INSERT INTO {_tableName} ({string.Join(", ", colunas)}) VALUES ({string.Join(", ", parametros)})";

        using var command = new SqliteCommand(sql, connection);

        // Preenche os valores dinamicamente
        foreach (var prop in propriedades.Where(p => p.Name != "Id"))
        {
            command.Parameters.AddWithValue("@" + prop.Name, prop.GetValue(entidade) ?? DBNull.Value);
        }

        command.ExecuteNonQuery();
    }

    public List<T> ListarTodos()
    {
        var lista = new List<T>();
        using var connection = DatabaseConfig.GetConnection();
        string sql = $"SELECT * FROM {_tableName}";

        using var command = new SqliteCommand(sql, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            T obj = new T();
            foreach (var prop in typeof(T).GetProperties())
            {
                // Mapeia o que vem do banco para o objeto C#
                var valor = reader[prop.Name];
                if (valor != DBNull.Value)
                {
                    prop.SetValue(obj, Convert.ChangeType(valor, prop.PropertyType));
                }
            }
            lista.Add(obj);
        }
        return lista;
    }

    // Pode seguir a mesma lógica para Atualizar e Excluir!
    public void Excluir(int id)
    {
        using var connection = DatabaseConfig.GetConnection();
        // O Reflection aqui é simples: apenas usamos o nome da tabela descoberto no construtor
        string sql = $"DELETE FROM {_tableName} WHERE Id = @Id";

        using var command = new SqliteCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", id);
        
        command.ExecuteNonQuery();
    }

    public void Atualizar(T entidade)
    {
        using var connection = DatabaseConfig.GetConnection();
        PropertyInfo[] propriedades = typeof(T).GetProperties();

        // Aqui a mágica do Reflection: 
        // Montamos a string "Nome = @Nome, Preco = @Preco..." dinamicamente
        var colunasParaAtualizar = propriedades
            .Where(p => p.Name != "Id") // Não atualizamos a chave primária
            .Select(p => $"{p.Name} = @{p.Name}");

        string sql = $"UPDATE {_tableName} SET {string.Join(", ", colunasParaAtualizar)} WHERE Id = @Id";

        using var command = new SqliteCommand(sql, connection);

        // Preenchemos todos os parâmetros, incluindo o Id para o WHERE
        foreach (var prop in propriedades)
        {
            command.Parameters.AddWithValue("@" + prop.Name, prop.GetValue(entidade) ?? DBNull.Value);
        }

        command.ExecuteNonQuery();
    }

    // Método extra para atender ao requisito de "Obtenção" (Select por ID)
    public T? ObterPorId(int id)
    {
        using var connection = DatabaseConfig.GetConnection();
        string sql = $"SELECT * FROM {_tableName} WHERE Id = @Id";

        using var command = new SqliteCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", id);
        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            T obj = new T();
            foreach (var prop in typeof(T).GetProperties())
            {
                var valor = reader[prop.Name];
                if (valor != DBNull.Value)
                {
                    prop.SetValue(obj, Convert.ChangeType(valor, prop.PropertyType));
                }
            }
            return obj;
        }
        return null;
    }
}