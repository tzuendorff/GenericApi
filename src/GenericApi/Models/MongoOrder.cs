using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace GenericApi.Classes;

public class MongoOrder
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }


    [Required]
    [StringLength(50)]
    public string CustomerFirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string CustomerLastName { get; set; } = string.Empty;

    [Required]
    public bool Approved { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "Order must contain at least one item.")]
    public List<Item> Items { get; set; } = new();

    public MongoOrder(Order ogirinalOrder)
    {

        Id = ogirinalOrder.Id;
        CustomerFirstName = ogirinalOrder.CustomerFirstName;
        CustomerLastName = ogirinalOrder.CustomerLastName;
        Approved = ogirinalOrder.Approved;
        Items = ogirinalOrder.Items.ToList();
    }

    public Order GetGeneralOrder()
    {
        return new Order
        {
            Id = Id,
            CustomerFirstName = CustomerFirstName,
            CustomerLastName = CustomerLastName,
            Approved = Approved,
            Items = Items
        };
    }
}

