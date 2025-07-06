using GenericApi.Classes;
using Microsoft.AspNetCore.Components.Routing;

namespace GenericApi.DataAccess
{
    public interface IDataAccess<T>
    {
        public Task<string> CreateEntity(T order);
        public Task<List<T>> ReadAllEntitiesByFilter(string id);
        public Task<int> UpdateEntity(T order);
        public Task<int> DeleteEntity(string id);
    }
}
