#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["MyRentMotorService/MyRentMotorService.API/MyRentMotorService.API.csproj", "MyRentMotorService/MyRentMotorService.API/"]
RUN dotnet restore "MyRentMotorService/MyRentMotorService.API/MyRentMotorService.API.csproj"
COPY "MyRentMotorService/." "MyRentMotorService/"
COPY ["../Shared", "Shared/"]
WORKDIR "/src/MyRentMotorService/MyRentMotorService.API"
RUN dotnet build "./MyRentMotorService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MyRentMotorService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

USER root

RUN apt-get update && apt-get install -y iputils-ping telnet curl wget postgresql-client \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

USER app

ENTRYPOINT ["dotnet", "MyRentMotorService.API.dll"]