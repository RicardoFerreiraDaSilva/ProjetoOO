namespace PizzariaFramework.Interfaces;

// O <T> permite que esse DAO sirva para Pizzas, Bebidas ou Pedidos
public interface IGenericDAO<T> where T : class
{
    void Inserir(T entidade);
    List<T> ListarTodos();
    void Atualizar(T entidade);
    void Excluir(int id);
}