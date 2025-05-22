using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// Setup PostgreSQL
var username = builder.AddParameter("username", secret: true);
var password = builder.AddParameter("password", secret: true);

var postgres = builder.AddPostgres("LayerHub", username, password, port: 5433)
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);
var postgresdb = postgres.AddDatabase("LayerHubWriteDb");

// Setup MongoDB
var mongo = builder.AddMongoDB("LayerHub-Mongo")
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);
var mongodb = mongo.AddDatabase("LayerHubReadDb");

// Setup API
var api = builder.AddProject<LayerHub_Api>("Api", launchProfileName: "https")
    .WithReference(mongodb, "MongoDbConnection")
    .WithReference(postgresdb, "DefaultConnection")
    .WaitFor(mongodb)
    .WaitFor(postgresdb);

// Setup Web
builder.AddProject<LayerHub_Web>("Web", launchProfileName: "https")
    .WithReference(api);

builder.Build().Run();