using GenericApi.Classes;
using Microsoft.AspNetCore.Components.Routing;

namespace GenericApi.DataAccess
{
    public interface IDataAccess<T>
    {
        public Task<string> CreateEntity(T order);
        public Task<List<T>> ReadAllEntitiesByFilter(string id);
        //public Task UpdateEntity(T order);
        //public Task DeleteEntity(string id);
    }
}
