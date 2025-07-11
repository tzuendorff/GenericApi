using GenericApi.Classes;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using GenericApi.Models.Configurations;
using MongoDB.Bson;

namespace GenericApi.DataAccess;

public class OrderMongoConnector : IDataAccess<Order>
{
    private readonly ILogger<OrderMongoConnector> _logger;
    private readonly IMongoCollection<Order> _ordersCollection;

    public OrderMongoConnector(IOptions<OrderDatabaseSettings> orderDatabaseSettings, ILogger<OrderMongoConnector> logger)
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

        await _ordersCollection.InsertOneAsync(order);
        await _ordersCollection.InsertOneAsync(orderToBeCreated);

        return orderToBeCreated.Id;
        }
        return order.Id;
    }

    public async Task<List<Order>> ReadAllEntitiesByFilter(string id)
    {
        var result = await _ordersCollection.Find(order => order.Id!.ToString().Contains(id)).ToListAsync(); // order.Id is never null, as this is an entry in the DB.
        return result;
    }

    public async Task<int> UpdateEntity(Order updatedOrder)
    {
        var result = await _ordersCollection.ReplaceOneAsync(order => order.Id == updatedOrder.Id, updatedOrder);
        return (int)result.ModifiedCount;
    }

    public async Task<int> DeleteEntity(string id)
    {
        var result = await _ordersCollection.DeleteOneAsync(order => order.Id == id);
        return (int)result.DeletedCount;
    }

}

