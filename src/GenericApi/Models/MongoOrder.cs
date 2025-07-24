using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace GenericApi.Classes;

public class MongoOrder : Order
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }


    public MongoOrder(Order ogirinalOrder)
    {

        Id = ogirinalOrder.OrderId;
        CustomerFirstName = ogirinalOrder.CustomerFirstName;
        CustomerLastName = ogirinalOrder.CustomerLastName;
        Approved = ogirinalOrder.Approved;
        Items = ogirinalOrder.Items.ToList();
    }

    public Order GetGeneralOrder()
    {
        return new Order
        {
            OrderId = Id,
            CustomerFirstName = CustomerFirstName,
            CustomerLastName = CustomerLastName,
            Approved = Approved,
            Items = Items
        };
    }
}

