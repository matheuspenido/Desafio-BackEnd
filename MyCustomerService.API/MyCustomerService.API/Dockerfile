#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MyCustomerService.API/MyCustomerService.API/MyCustomerService.API.csproj", "MyCustomerService.API/MyCustomerService.API/"]
RUN dotnet restore "MyCustomerService.API/MyCustomerService.API/MyCustomerService.API.csproj"
COPY "MyCustomerService.API/." "MyCustomerService.API/"
COPY ["../Shared", "./Shared"]
WORKDIR "/src/MyCustomerService.API/MyCustomerService.API"
RUN dotnet build "MyCustomerService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MyCustomerService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

USER root

RUN apt-get update && apt-get install -y iputils-ping telnet curl wget postgresql-client \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

USER app

ENTRYPOINT ["dotnet", "MyCustomerService.API.dll"]