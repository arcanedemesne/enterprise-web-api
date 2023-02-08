# enterprise-web-api

# Enterprise Solution :: Web API

## Project Overview
### Description
This is a demo project to implement the lastest techiniques and best practices in a .NET Core WebAPI using .NET Core 7, EF Core 7, PostgreSQL, Redis, & Docker

---

## Development Environment Setup
### Prerequisites
1. Install Visual Studio 2022 
2. Install client-side tools, and run development client-side build.  See: [src/server/_static/README.md](README.md)
3. Review and follow the [documentation/Code Standards and Practices.md](Code Standards and Practices document) in the documentation folder before starting.
4. Review the [architecture diagram](TODO: create lucid chart representation of architecture) and [application architecture documentation](/documentation/Web Application Architecture.md) to familiarize yourself with the overall organization of the application before starting.

## Configure User Secrets
Application secrets are stored in [Mainframe](TODO: create mainframe.nerdery for secrets) and should be configured locally on your machine using [App Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-7.0&tabs=windows).

For the following secrets, you can configure one of three ways:

1. Command Line: `dotnet user-secrets set "SECRET KEY" "SECRET VALUE" --project "\<path-to-app>\src\server\Enterprise.Solution"`
2. Right-clicking on the Enterprise.Solution project in Visual Studio and choosing `Manage User Secrets`.
3. Manually: `%APPDATA%\Microsoft\UserSecrets\Enterprise.Solution\secrets.json`

### Database
* `ConnectionStrings:PlatformDatabase` - Database connection string
* `ConnectionStrings:Cache` - Redis database connection string

#### Other Setup Items
1. If you run into the following error using IISExpress: `UnauthorizedAccessException: Access to the path 'C:\Program Files\IIS Express\tempkey.rsa' is denied.` Modify permissions on the IISExpress folder to give your account write permissions so it can create the temporary RSA key and store it in that folder.
2. If you encounter an error concerning untrusted certificates, run `dotnet dev-certs https --trust`

### Completed secrets.json
A completed secrets.json should look like this (with relevant secrets populated):

```
{
  "Kestrel:Certificates:Development:Password": "",
  "ConnectionStrings: {
    "SolutionDatabase": "",
    "Cache": ""
   },
  "SolutionSettings": {
    "StorageSettings": {
      "AccountKey": "",
      "AccountSecret": "",
      "StorageConnectionEndpoint": ""
    },
    "ThumbnailSettings": {
      "ServiceBusConnectionString": ""
    }
  } 
}
```

---

## Higher Environment Setup
When deploying to a higher environment, the following values need to be additionally configured, in addition to the values above:

## Architecture Implementation

See the Architecture document for a higher-level overview of the architectural
principals and practices used in the application.

### APIs

#### Swagger API
Swagger API Information can be found at `/swagger` after running project on development. 

#### Postman
Our [Postman workspace](https://XXX) contains several documented requests and examples that interact with the various APIs used in this project. Currently the *Development* environment exists, so ensure this is selected prior to executing requests.

---

### Database

##### Naming Standards Summary

Here is a summary of the standards being implemented:

* Use Pascal Case
* Use a full word suffix on each column denoting it's type or intention whenever * possible (Flag, Amount, Quantity, Indicator, Id)
* If a column starts with reserved keyword from ANSI Sql, T-SQL or Oracle SQL, prefix with the table name
* If a column name matches the Table Name a prefix will be added
* If a table has a composite primary key, there should be another column used as a unique row identifier.
* Plural table names for join tables only if no other appropriate name can be deteremined.

#### Tables
* 

#### Data Interaction

The [CQRS](https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/microservice-ddd-cqrs-patterns/) pattern is utilized within our project to query and modify data, supported by [MediatR](https://github.com/jbogard/MediatR). *Commands* (requests that modifies state) should be defined within the `Kirkland.SubPlatform.Core.Data.Commands` namespace, organized by feature. *Queries* (requests that do not modify state at all) should be defined within the `Kirkland.SubPlatform.Core.Data.Queries` namespace. Following the MediatR pattern, any logic to support the request should be defined within the `Handle` method of the requests's `Handler` class.

Validation happens per request at the class/request level. Each request should have a `Validator` class created whose constructor defines the rules set, supported by [FluentValidation](https://github.com/JeremySkinner/FluentValidation). This validation occurs with each request execution.

### Mailhog

Mailhog is used to trap emails sent from the application. Mailhog is installed as a Docker container using docker-compose up.
The Mailhog mail interface is exposed on port 1025.
Mailhog also has a web interface to allow viewing of the email messages sent to and received by Mailhog. The web interface is accesible
via localhost:8025.
You can send a test email to Mailhog using the following PowerShell command:

Send-MailMessage -To “recipient@test.com” -From “sender@test.com” -Subject "Test email" -SmtpServer “localhost” -Port 1025


