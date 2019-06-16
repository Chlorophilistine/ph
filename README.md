# Test Submission Readme

## Pre-requisites
- .NET Framework 7.2
- Sql Server Express 2016 LocalDB ([install instructions](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-2017#install-localdb))
- Visual Studio 2019 to build

The solution will deploy a seeded database locally via LocalDB on first run, and in best development tradition the initialiser will drop the DB on entity changes.

The solution is separated into projects
- CustomerApp, for web API controllers and logic
- CustomerApp.DataAcess, for data manipulation & persistence concerns

There solution folder "UnitTests" featues a further 2 NUnit unit test projects supporting the app.

## Quick Access - Running the API
Build in Visual Studio, then run with or without debugging as applicable. The API is configured to be avialble on [https://localhost:44399/api/v1/customers](https://localhost:44399/api/v1/customers).

## URL Taxonomy
The API is a lightweight ReST-style, and provides the functionality described in the brief. Customers can be listed, individual customer details retrieved, customer status updated, and notes associated with a customer updated and created. In addition it allows for creation and deletion of customers, as this could be reasonable inferred from the brief.

/api/v1/customers <- GET

/api/v1/customers/{id} <- GET, POST, DELETE

/api/v1/customers/{id}/status <- POST

/api/v1/customers/{id}/notes <- GET

/api/v1/notes <- PUT, POST

The HTTP verbs assume the usual ReST meaning. The serializer is configured to return XML, or JSON, so you can uesr Postman or RestedClient or your choice of tool to probe the API.

The API observes the bare minimum in versioning (v1) to allow for future expansion and revision in a fairly painless manner (as opposed to v1 being the root).

## DI in the API
The API and data layer are assembled using the MEF composition container, using declaritive attribute based configuration. Please see CusotomerApp.MefContainer for details of how the container catalog is defined, or if adding dependenices.

## DI responsibilities
It is important that any additional controllers or repositories, or any other dependency supplied by MEF, that will be request-scoped use the following attribute at class level

```c#
[PartCreationPolicy(CreationPolicy.NonShared)]
```
to ensure they are disposed in the scope of the request.