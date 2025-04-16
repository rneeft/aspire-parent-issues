var builder = DistributedApplication.CreateBuilder(args);

var sqlServer = builder
    .AddSqlServer("sql", port: 1234)
    .WithDataVolume("Data")
    .WithLifetime(ContainerLifetime.Persistent);

var db1 = sqlServer.AddDatabase("db1");
var migration1 = builder
    .AddProject<Projects.ConsoleApp1>("db1-migrations")
    .WithReference(db1)
    .WaitFor(db1)
    .WithParentRelationship(db1);

var db2 = sqlServer.AddDatabase("db2");
var migration2 = builder
    .AddProject<Projects.ConsoleApp2>("db2-migrations")
    .WithReference(db2)
    .WaitFor(db2)
    .WithParentRelationship(db2)
    .WithExplicitStart();

builder.Build().Run();
