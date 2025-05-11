using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("username", secret: true);
var password = builder.AddParameter("password", secret: true);

var postgres = builder.AddPostgres("LayerHub", username, password, port: 5433);
var postgresdb = postgres.AddDatabase("DefaultConnection");

builder.AddProject<LayerHub_Api>("Api", launchProfileName: "https")
    .WithReference(postgresdb)
    .WaitFor(postgresdb);

builder.AddProject<LayerHub_Web>("Web", launchProfileName: "https");

builder.Build().Run();