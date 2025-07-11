using GenericApi.Classes;

namespace GenericApi.BusinessLogic;
public interface IBusinessLogic<T>
{
    public Task<string> CreateEntity(T entity);
    public Task<List<T>> ReadAllEntitiesByFilter(string id);
    public Task<int> UpdateEntity(T entity);
    public Task<int> DeleteEntity(string id);
}