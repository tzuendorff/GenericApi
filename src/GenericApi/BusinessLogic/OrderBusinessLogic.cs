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
    public List<Order> ReadAllEntity()
    {
        _logger.LogInformation($"Reading all orders with");
        var orders = _dataAccess.ReadAllEntity().Result;
        _logger.LogInformation($"Succesfully read order. Number of entries read: {orders.Count}");
        return orders;
    }

    //public void UpdateEntity(Order order)
    //{
    //    _logger.LogInformation($"Updaing order {order}");
    //    _dataAccess.UpdateEntity(order);
    //    _logger.LogInformation($"Succesfully updated order with Id {order.Id}");
    //}

    //public void DeleteEntity(string id)
    //{
    //    _logger.LogInformation($"Deleting order {id}");
    //    _dataAccess.DeleteEntity(id);
    //    _logger.LogInformation($"Succesfully deleted order with Id {id}");
    //}
}
