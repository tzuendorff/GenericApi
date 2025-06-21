namespace GenericApi.Classes
{
    public class Order 
    {
        public required int Id { get; set; }
        public required string CustomerFirstName { get; set; }
        public required string CustomerLastName { get; set; }
        public required bool Approved { get; set; }
        public required List<Item> Items { get; set; }
    }
}
