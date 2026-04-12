using System.Reflection;

namespace GenericFramework.Data;

public static class MenuEngine
{
    public static object PreencherObjetoDinamico(Type tipo)
    {
        // Cria uma instância vazia da classe (ex: nova Pizza)
        var objeto = Activator.CreateInstance(tipo)!;
        
        // Pega todas as propriedades públicas (Nome, Preco, Cargo, etc)
        // Ignoramos o "Id" porque o SQLite gera sozinho
        var propriedades = tipo.GetProperties()
                               .Where(p => p.Name != "Id");

        Console.WriteLine($"\n--- Preenchendo dados de {tipo.Name} ---");

        foreach (var prop in propriedades)
        {
            Console.Write($"{prop.Name}: ");
            string entrada = Console.ReadLine() ?? "";

            try
            {
                // Converte o texto do teclado para o tipo da propriedade (decimal, int, bool...)
                object valorConvertido;
                
                if (prop.PropertyType == typeof(bool))
                {
                    valorConvertido = entrada.ToUpper() == "S";
                }
                else
                {
                    valorConvertido = Convert.ChangeType(entrada, prop.PropertyType);
                }

                prop.SetValue(objeto, valorConvertido);
            }
            catch
            {
                Console.WriteLine($"⚠️ Erro: Valor inválido para {prop.Name}.");
            }
        }

        return objeto;
    }
}