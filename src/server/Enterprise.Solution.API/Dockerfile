#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Enterprise.Solution.API/Enterprise.Solution.API.csproj", "Enterprise.Solution.API/"]
RUN dotnet restore "Enterprise.Solution.API/Enterprise.Solution.API.csproj"
COPY . .
WORKDIR "Enterprise.Solution.API"
RUN dotnet build "Enterprise.Solution.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Enterprise.Solution.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS http://*:5000

ENTRYPOINT ["dotnet", "Enterprise.Solution.API.dll"]