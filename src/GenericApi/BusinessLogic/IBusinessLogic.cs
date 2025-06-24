using GenericApi.Classes;

namespace GenericApi.BusinessLogic;
public interface IBusinessLogic<T>
{
    public string CreateEntity(Order order);
    public List<Order> ReadAllEntitiesByFilter(string id);
    //public void UpdateEntity(Order order);
    //public void DeleteEntity(string id);
}