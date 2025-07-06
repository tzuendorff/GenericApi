using GenericApi.Classes;
using GenericApi.Controllers;
using GenericApi.DataAccess;

namespace GenericApi.BusinessLogic;

public class OrderBusinessLogic : IBusinessLogic<Order>
{
    private readonly ILogger<GenericController> _logger;
    private readonly IDataAccess<Order> _dataAccess;

    public OrderBusinessLogic(ILogger<GenericController> logger, IDataAccess<Order> dataAccess)
    {
        _logger = logger;
        _dataAccess = dataAccess;
    }

    public string CreateEntity(Order order)
    {
        _logger.LogInformation($"Creating new Order {order}");
        var orderId = _dataAccess.CreateEntity(order).Result;
        _logger.LogInformation($"Created new Order with Id {orderId}");
        return orderId;
    }
    public List<Order> ReadAllEntitiesByFilter(string orderId)
    {
        _logger.LogInformation($"Reading order with Id {orderId}");
        var order = _dataAccess.ReadAllEntitiesByFilter(orderId).Result;
        _logger.LogInformation($"Succesfully read order. {order}");
        return order;
    }

    public int UpdateEntity(Order order)
    {
        _logger.LogInformation($"Updaing order {order.Id} with {order}");
        var numberOfModifiedEntities = _dataAccess.UpdateEntity(order).Result;
        _logger.LogInformation($"Succesfully updated {numberOfModifiedEntities} order/s with Id {order.Id}");
        return numberOfModifiedEntities;
    }

    public int DeleteEntity(string id)
    {
        _logger.LogInformation($"Deleting order {id}");
        var numberOfDeletedEntities = _dataAccess.DeleteEntity(id).Result;
        _logger.LogInformation($"Succesfully deleted {numberOfDeletedEntities} order/s with Id {id}");
        return numberOfDeletedEntities;
    }
}
