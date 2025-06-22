using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace GenericApi.Classes;

public class Order 
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public required string CustomerFirstName { get; set; }
    public required string CustomerLastName { get; set; }
    public required bool Approved { get; set; }
    public required List<Item> Items { get; set; }
}
