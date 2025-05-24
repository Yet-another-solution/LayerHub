using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// Setup PostgreSQL
var username = builder.AddParameter("username", secret: true);
var password = builder.AddParameter("password", secret: true);

var postgres = builder.AddPostgres("LayerHubPostgreSql", username, password, port: 5433)
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);
var postgresdb = postgres.AddDatabase("LayerHubWriteDb");

// Setup MongoDB
var mongo = builder.AddMongoDB("LayerHubMongo")
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);
var mongodb = mongo.AddDatabase("LayerHubReadDb");

// Setup API
var api = builder.AddProject<LayerHub_Api>("Api")
    .WithReference(mongodb, "MongoDbConnection")
    .WithReference(postgresdb, "DefaultConnection")
    .WaitFor(mongodb)
    .WaitFor(postgresdb);

// Setup Web
builder.AddProject<LayerHub_Web>("Web")
    .WithExternalHttpEndpoints()
    .WithReference(api);

builder.AddDockerComposeEnvironment("compose");

builder.Build().Run();