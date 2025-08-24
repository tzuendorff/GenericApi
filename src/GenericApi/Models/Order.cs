using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace GenericApi.Classes;


using System.ComponentModel.DataAnnotations;

public class Order 
{
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
    [MinLength(0, ErrorMessage = "Order must contain at least one item.")]
    public List<Item> Items { get; set; } = new();
}
