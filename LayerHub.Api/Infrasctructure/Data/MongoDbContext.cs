using LayerHub.Shared.ReadDocuments;
using MongoDB.Driver;

namespace LayerHub.Api.Infrasctructure.Data;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public IMongoCollection<MapLayerDocument> MapLayers { get; }

    public MongoDbContext(IConfiguration configuration)
    {
        _database = new MongoClient(configuration.GetConnectionString("MongoDbConnection")).GetDatabase("LayerHubReadDb");

        // Register Collections
        MapLayers = _database.GetCollection<MapLayerDocument>("Layers");
    }
}