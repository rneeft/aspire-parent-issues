using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// Fake a SQL Server
var app1 = builder
    .AddProject<ConsoleApp1>("sql-app")
    .WithArgs("-1");

var db1App = builder
    .AddProject<Projects.ConsoleApp1>("db1-app")
    .WaitFor(app1)
    .WithParentRelationship(app1)
    .WithArgs("-1");

var db2App = builder
    .AddProject<Projects.ConsoleApp1>("db2-app")
    .WaitFor(app1)
    .WithParentRelationship(app1)
    .WithArgs("-1");

var migration1App = builder
    .AddProject<Projects.ConsoleApp1>("db1-migrations-app")
    .WithArgs("2000")
    .WithReference(db1App)
    .WaitFor(db1App)
    .WithParentRelationship(db1App);

var migration2App = builder
    .AddProject<Projects.ConsoleApp1>("db2-migrations-app")
    .WithArgs("2000")
    .WithReference(db2App)
    .WaitFor(db2App)
    .WithParentRelationship(db2App)
    .WithExplicitStart();

// Real SQL server

var sqlServer = builder
    .AddSqlServer("sql", port: 1234)
    .WithDataVolume("Data")
    .WithLifetime(ContainerLifetime.Persistent);

var db1 = sqlServer.AddDatabase("db1");
var migration1 = builder
    .AddProject<Projects.ConsoleApp1>("db1-migrations")
    .WithArgs("2000")
    .WithReference(db1)
    .WaitFor(db1)
    .WithParentRelationship(db1);

var db2 = sqlServer.AddDatabase("db2");
var migration2 = builder
    .AddProject<Projects.ConsoleApp2>("db2-migrations")
    .WithArgs("2000")
    .WithReference(db2)
    .WaitFor(db2)
    .WithParentRelationship(db2)
    .WithExplicitStart();


builder.Build().Run();
