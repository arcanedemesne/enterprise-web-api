# enterprise-web-api

# Enterprise Solution :: Web API

## Project Overview
### Description

This is a demo project to implement the lastest techiniques and best practices in a .NET Core WebAPI using .NET Core 7, EF Core 7, PostgreSQL, Redis, & Docker

---


## Development Environment Setup
###Prerequisites

1. Install Visual Studio 2022 or later & .NET Core 7 SDK or later
2. Run Poweshell Scripts in [src/server/] in order, this will install openssl and nginx certificates, and docker desktop
3. Inside [src/server/] Run docker-compose build, and then docker-compose up. Everything should just start running :)
   [Note: make sure Docker Desktop is Running]


### Find your Services

1. pgAdmin  @ http://localhost:5050/browser/
2. Keycloak @ http://localhost:8080/auth/
3. Mailhog  @ http://localhost:8025/
4. .NET API @ https://localhost/swagger/index.html


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


