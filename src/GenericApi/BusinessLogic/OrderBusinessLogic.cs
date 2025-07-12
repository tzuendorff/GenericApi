using GenericApi.Classes;
using GenericApi.Controllers;
using GenericApi.DataAccess;

namespace GenericApi.BusinessLogic;

public class OrderBusinessLogic : IBusinessLogic<Order>
{
    private readonly ILogger<GenericController> _logger;
    private readonly IGenericRepository<Order> _dataAccess;

    public OrderBusinessLogic(ILogger<GenericController> logger, IGenericRepository<Order> dataAccess)
    {
        _logger = logger;
        _dataAccess = dataAccess;
    }

    public async Task<string> CreateEntity(Order order)
    {
        var orderId = await _dataAccess.CreateEntity(order);
        return orderId;
    }

    public async Task<List<Order>> ReadAllEntitiesByFilter(string orderId)
    {
        var orders = await _dataAccess.ReadAllEntitiesByFilter(orderId);
        return orders;
    }

    public async Task<int> UpdateEntity(Order order)
    {
        var numberOfModifiedEntities = await _dataAccess.UpdateEntity(order);
        return numberOfModifiedEntities;
    }

    public async Task<int> DeleteEntity(string id)
    {        
        var numberOfDeletedEntities = await _dataAccess.DeleteEntity(id);
        return numberOfDeletedEntities;
    }
}
