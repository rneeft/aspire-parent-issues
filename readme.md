I encountered an issue where a resource becomes stuck due to its parent relationship including the `WithExplicitStart`. Specifically, after running the AppHost, all resources, including the db2-migrations resource, turn green. However, the db2-migrations seems also be running and the resource cannot be stopped, making it impossible to manage properly.

## Code snippet

The core of the code is as follows: 

```csharp
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
```

## Steps to Reproduce:
1.	Clone the my repository: https://github.com/rneeft/aspire-parent-issues
2.	Run the AppHost application.
3.	Observe that all resources, including db2-migrations, turn green.

Eventually, all resources become available, including db2-migrations.

![All](img/ExplicitStart%20running.png)

## Issue

The resource db2-migrations should not become 'Running' and when trying to stop the db2-migrations resource a error becomes visible.

![Err](img/Stopping.png)

```
Error executing command 'resource-stop'.
k8s.Autorest.HttpOperationException: Operation returned an invalid status code 'NotFound', response body {"kind":"Status","apiVersion":"v1","metadata":{},"status":"Failure","message":"executables.usvc-dev.developer.microsoft.com \"db2-migrations-ghbztrct\" not found","reason":"NotFound","details":{"name":"db2-migrations-ghbztrct","group":"usvc-dev.developer.microsoft.com","kind":"executables"},"code":404}
```

## Expected Behavior

The db2-migrations resource should not enter the Running state unless explicitly started.

## Actual behavior

The db2-migrations resource becomes stuck in the Running state and cannot be stopped.
