using GenericApi.BusinessLogic;
using GenericApi.Classes;
using GenericApi.DataAccess;
using GenericApi.Models.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Bind CORS settings from configuration
builder.Services.Configure<CorsSettings>(builder.Configuration.GetSection("Cors"));
var corsSettings = builder.Configuration.GetSection("Cors").Get<CorsSettings>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Use PascalCase property naming policy
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("ConfiguredCorsPolicy", policy =>
    {
        policy
            .WithOrigins(corsSettings.AllowedOrigins)
            .WithMethods(corsSettings.AllowedMethods)
            .WithHeaders(corsSettings.AllowedHeaders);
    });
});


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
builder.Services.Configure<OrderDatabaseSettings>(
builder.Configuration.GetSection("MongoDatabase"));

builder.Services.AddSingleton<IGenericRepository<Order>, OrderMongoRepository>();
builder.Services.AddSingleton<IBusinessLogic<Order>, OrderBusinessLogic>();

// Add health checks
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use the configured CORS policy
app.UseCors("ConfiguredCorsPolicy");

app.UseAuthorization();
app.MapGet("/debug/cors", (IConfiguration config) =>
{
    var cors = config.GetSection("Cors").Get<CorsSettings>();
    return Results.Json(cors);
});

app.MapControllers();
// Map health check endpoint
app.MapHealthChecks("/check");

app.Run();
