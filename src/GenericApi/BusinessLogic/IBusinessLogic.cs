using GenericApi.Classes;

namespace GenericApi.BusinessLogic;
public interface IBusinessLogic<T>
{
    public string CreateEntity(Order order);
    public List<Order> ReadAllEntitiesByFilter(string id);
    public int UpdateEntity(Order order);
    public int DeleteEntity(string id);
}