using GenericApi.Classes;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using GenericApi.Models.Configurations;

namespace GenericApi.DataAccess;

public class OrderMongoRepository : IGenericRepository<Order>
{
    private readonly ILogger<OrderMongoRepository> _logger;
    private readonly IMongoCollection<MongoOrder> _ordersCollection;

    public OrderMongoRepository(IOptions<OrderDatabaseSettings> orderDatabaseSettings, ILogger<OrderMongoRepository> logger)
    {
        var clientSettings = new MongoClientSettings();
        clientSettings.Credential = MongoCredential.CreateCredential(orderDatabaseSettings.Value.AuthenticationDatabaseName, orderDatabaseSettings.Value.Username, orderDatabaseSettings.Value.Password);
        clientSettings.Server = new MongoServerAddress(orderDatabaseSettings.Value.Host, orderDatabaseSettings.Value.Port); // Assuming default port 27017 for MongoDB 
        
        var mongoClient = new MongoClient(clientSettings);

        var mongoDatabase = mongoClient.GetDatabase(orderDatabaseSettings.Value.DatabaseName);

        _ordersCollection = mongoDatabase.GetCollection<MongoOrder>(orderDatabaseSettings.Value.OrderCollectionName);

        _logger = logger;
    }
    public async Task<string> CreateEntity(Order orderToBeCreated)
    {
        var mongoOrder = new MongoOrder(orderToBeCreated); // Id is null
        await _ordersCollection.InsertOneAsync(mongoOrder);
        // mongoOrder.Id now contains the generated ObjectId
        _logger.LogInformation($"Creating new Order {mongoOrder}");
        _logger.LogInformation($"Created new Order with Id {mongoOrder.Id}");

        return mongoOrder.Id.ToString();
    }

    public async Task<List<Order>> ReadAllEntitiesByFilter(string id)
    {
        _logger.LogInformation($"Reading order with Id {id}");
        var result = await _ordersCollection.Find(order => order.Id!.ToString().Contains(id)).ToListAsync(); // order.Id is never null, as this is an entry in the DB.
        _logger.LogInformation($"Succesfully read order. {result}");

        var generalOrders = new List<Order>();

        foreach(MongoOrder order in result)
        {
            generalOrders.Add(order.GetGeneralOrder());
        }

        return generalOrders;
    }

    public async Task<int> UpdateEntity(Order updatedOrder)
    {
        var updateMongOrder = new MongoOrder(updatedOrder);
        _logger.LogInformation($"Updaing order {updateMongOrder.Id} with {updateMongOrder}");
        var result = await _ordersCollection.ReplaceOneAsync(order => order.Id == updateMongOrder.Id, updateMongOrder);
        _logger.LogInformation($"Succesfully updated {result.ModifiedCount} order/s with Id {updateMongOrder.Id}");

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

