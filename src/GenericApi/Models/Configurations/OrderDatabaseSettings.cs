namespace GenericApi.Models.Configurations;

public class OrderDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string OrderCollectionName { get; set; } = null!;

    public string AuthenticationDatabaseName { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

}
