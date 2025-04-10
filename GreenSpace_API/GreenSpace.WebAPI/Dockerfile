# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN apt-get update && \
    apt-get install -y --no-install-recommends \
    fonts-dejavu-core fonts-liberation fonts-freefont-ttf && \
    apt-get clean && rm -rf /var/lib/apt/lists/*
#USER $APP_UID
WORKDIR /app
COPY appsettings*.json /app/
ENV ASPNETCORE_URLS=http://+:8080;https://+:8081
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GreenSpace.WebAPI/GreenSpace.WebAPI.csproj", "GreenSpace.WebAPI/"]
COPY ["GreenSpace.Application/GreenSpace.Application.csproj", "GreenSpace.Application/"]
COPY ["GreenSpace.Domain/GreenSpace.Domain.csproj", "GreenSpace.Domain/"]
COPY ["GreenSpace.Infrastructure/GreenSpace.Infrastructure.csproj", "GreenSpace.Infrastructure/"]
RUN dotnet restore "./GreenSpace.WebAPI/GreenSpace.WebAPI.csproj"
COPY . .
WORKDIR "/src/GreenSpace.WebAPI"
RUN dotnet build "./GreenSpace.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GreenSpace.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "GreenSpace.WebAPI.dll"]
ENTRYPOINT ["dotnet", "GreenSpace.WebAPI.dll", "--urls", "http://0.0.0.0:8080"]