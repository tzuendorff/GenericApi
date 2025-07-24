using GenericApi.Classes;
using GenericApi.DataAccess;

namespace TestGenericApi;

public class MockOrderMongoConnector : IGenericRepository<Order>
{
    public bool shouldThrowGeneralException = false;
    
    public Task<string> CreateEntity(Order orderToBeCreated)
    {
        if (shouldThrowGeneralException)
        {
            shouldThrowGeneralException = false;
            throw new Exception("General exception for testing purposes.");
        }

        if (orderToBeCreated.Id == "")
        {
            orderToBeCreated.Id = null;
        }
        if (orderToBeCreated.Id != null)
        {
            throw new ArgumentException();
        }
        return Task.Run(() => "1111");
    }

    public Task<List<Order>> ReadAllEntitiesByFilter(string id)
    {
        if (shouldThrowGeneralException)
        {
            shouldThrowGeneralException = false;
            throw new Exception("General exception for testing purposes.");
        }

        if (id == "notFoundId")
        {
            return Task.Run(() => new List<Order>()); // Return an empty list instead of null
        }

        return Task.Run(() => new List<Order>
        {
            new Order
            {
                Id = "1111",
                CustomerFirstName = "Alice",
                CustomerLastName = "Smith",
                Approved = true,
                Items = new List<Item>
                {
                    new Item { Id = "1", Amount = 2 },
                    new Item { Id = "2", Amount = 1 }
                }
            },
            new Order
            {
                Id = "2222",
                CustomerFirstName = "Bob",
                CustomerLastName = "Johnson",
                Approved = false,
                Items = new List<Item>
                {
                    new Item { Id = "3", Amount = 5 }
                }
            }
        });   
        
    }

    public Task<int> UpdateEntity(Order updatedOrder)
    {
        if (shouldThrowGeneralException)
        {
            shouldThrowGeneralException = false;
            throw new Exception("General exception for testing purposes.");
        }

        if (updatedOrder.Id == "someBadId")
        {
            throw new FormatException();
        }
        if (updatedOrder.Id == "notFoundId")
        {
            return Task.Run(() => 0);
        }
        return Task.Run(() => 1);
    }

    public Task<int> DeleteEntity(string deletedId)
    {
        if (shouldThrowGeneralException)
        {
            shouldThrowGeneralException = false;
            throw new Exception("General exception for testing purposes.");
        }

        if (deletedId == "someBadId")
        {
            throw new FormatException();
        }
        if (deletedId == "notFoundId")
        {
            return Task.Run(() => 0);
        }
        return Task.Run(() => 1);
    }

}
