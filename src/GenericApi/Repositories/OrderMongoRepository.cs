using GenericApi.Classes;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using GenericApi.Models.Configurations;

namespace GenericApi.DataAccess;

public class OrderMongoRepository : IGenericRepository<Order>
{
    private readonly ILogger<OrderMongoRepository> _logger;
    private readonly IMongoCollection<Order> _ordersCollection;

    public OrderMongoRepository(IOptions<OrderDatabaseSettings> orderDatabaseSettings, ILogger<OrderMongoRepository> logger)
    {
        var clientSettings = new MongoClientSettings();
        clientSettings.Credential = MongoCredential.CreateCredential(orderDatabaseSettings.Value.AuthenticationDatabaseName, orderDatabaseSettings.Value.Username, orderDatabaseSettings.Value.Password);

        var mongoClient = new MongoClient(clientSettings);

        var mongoDatabase = mongoClient.GetDatabase(orderDatabaseSettings.Value.DatabaseName);

        _ordersCollection = mongoDatabase.GetCollection<Order>(orderDatabaseSettings.Value.OrderCollectionName);

        _logger = logger;
    }
    public async Task<string> CreateEntity(Order orderToBeCreated)
    {
        //If property "Id" is provided but empty, set it to null.
        // This automatically creates a new id, when creating it in the database.
        if (orderToBeCreated.Id == "")
        {
            orderToBeCreated.Id = null; 
        }
        if (orderToBeCreated.Id != null)
        {
            _logger.LogError($"Order not inserted into Mongo Collection. Order Id must not be set by caller. {orderToBeCreated}");
            throw new ArgumentException("Id must be empty. The database creates a new Id when created. That Id is returned from this method.");
        }

        _logger.LogInformation($"Creating new Order {orderToBeCreated}");
        await _ordersCollection.InsertOneAsync(orderToBeCreated);
        _logger.LogInformation($"Created new Order with Id {orderToBeCreated.Id}");

        return orderToBeCreated.Id;
    }

    public async Task<List<Order>> ReadAllEntitiesByFilter(string id)
    {
        _logger.LogInformation($"Reading order with Id {id}");
        var result = await _ordersCollection.Find(order => order.Id!.ToString().Contains(id)).ToListAsync(); // order.Id is never null, as this is an entry in the DB.
        _logger.LogInformation($"Succesfully read order. {result}");

        return result;
    }

    public async Task<int> UpdateEntity(Order updatedOrder)
    {
        _logger.LogInformation($"Updaing order {updatedOrder.Id} with {updatedOrder}");
        var result = await _ordersCollection.ReplaceOneAsync(order => order.Id == updatedOrder.Id, updatedOrder);
        _logger.LogInformation($"Succesfully updated {result.ModifiedCount} order/s with Id {updatedOrder.Id}");

        return (int)result.ModifiedCount;
    }

    public async Task<int> DeleteEntity(string id)
    {
        _logger.LogInformation($"Deleting order {id}");
        var result = await _ordersCollection.DeleteOneAsync(order => order.Id == id);
        _logger.LogInformation($"Succesfully deleted {result.DeletedCount} order/s with Id {id}");

        return (int)result.DeletedCount;
    }

}

