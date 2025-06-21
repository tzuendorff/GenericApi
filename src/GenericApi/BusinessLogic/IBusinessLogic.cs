using GenericApi.Classes;

namespace GenericApi.BusinessLogic;
public interface IBusinessLogic<T>
{
    public int CreateEntity(Order order);
    public Order ReadOneEntity(int id);
    public List<Order> ReadAllEntity();
    public void UpdateEntity(Order order);
    public void DeleteEntity(int id);
}