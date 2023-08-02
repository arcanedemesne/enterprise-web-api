# enterprise-web-api

# Enterprise Solution :: Web API

## Project Overview
### Description

This is a demo project to implement the lastest techiniques and best practices in a .NET Core WebAPI using .NET Core 7, EF Core 7, PostgreSQL, Redis, & Docker. The front end is React, leveraging React-Router-Don, Reduux Toolkit, and JoyUI from MUI.

---


## Development Environment Setup
###Prerequisites

1. Install Git, NVM for Windows, and latest LTS Node through NVM.
2. Install Visual Studio 2022 or later (https://visualstudio.microsoft.com/thank-you-downloading-visual-studio/?sku=Community&channel=Release&version=VS2022)
3. Install .NET Core 7 SDK or later (https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
4. Install openssl (https://slproweb.com/products/Win32OpenSSL.html)
5. Install Docker Desktop (https://www.docker.com/products/docker-desktop/)
6. Run the script 'setup-dev-certs.ps1' in [src/server/] in OpenSSL Terminal, this will install the nginx certificates
7. Inside [src/server/] Run docker-compose build, and then docker-compose up. Everything should start running.
   [Note: make sure Docker Desktop is Running]
8. Once Keycloak is running, import the realm (using 'enterprise-solution-realm.json' in [src/server/]).
   This will contain a test user with admin priviledges, and a swagger user for testing in swagger.
9. Run the front end in [src/client] using npm install & npm start. Enjoy :)

### Find your Services

1. pgAdmin  @ http://localhost:5050/browser/
2. Keycloak @ http://localhost:8080/auth/
3. Mailhog  @ http://localhost:8025/
4. .NET API @ https://localhost/swagger/index.html
5. React FE @ http://localhost:3000


### PostgreSQL Tools

pgAdmin is the most popular and feature rich Open Source administration and development platform for PostgreSQL,
the most advanced Open Source database in the world.
pgAdmin may be used on Linux, Unix, macOS and Windows to manage PostgreSQL and EDB Advanced Server 10 and above. 

pgAdmin is installed as a Docker container using docker-compose up.
The pgAdmin databases interface is exposed on port 5432.
pgAdmin also has a web interface to allow viewing of the databases.

The web interface is accesible via localhost:5050/browser/


### Keycloak

Keycloak provides single-sign out, which means users only have to logout once to be logged-out of all applications that use Keycloak.
Identity Brokering and Social Login.
Enabling login with social networks is easy to add through the admin console.

Keycloak is installed as a Docker container using docker-compose up.
The Keycloak interface is exposed on port 8080.

The web interface is accesible via localhost:8080/auth/


### Mailhog

Mailhog is used to trap emails sent from the application.

Mailhog is installed as a Docker container using docker-compose up.
The Mailhog mail interface is exposed on port 1025.
Mailhog also has a web interface to allow viewing of the email messages sent to and received by Mailhog.

The web interface is accesible via localhost:8025/

You can send a test email to Mailhog using the following PowerShell command:
Send-MailMessage -To “recipient@test.com” -From “sender@test.com” -Subject "Test email" -SmtpServer “localhost” -Port 1025


