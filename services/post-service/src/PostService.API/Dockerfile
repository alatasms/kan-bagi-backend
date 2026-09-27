# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
#USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/PostService.API/PostService.API.csproj", "src/PostService.API/"]
COPY ["src/PostService.Application/PostService.Application.csproj", "src/PostService.Application/"]
COPY ["src/PostService.Domain/PostService.Domain.csproj", "src/PostService.Domain/"]
COPY ["src/PostService.Infrastructure/PostService.Infrastructure.csproj", "src/PostService.Infrastructure/"]
RUN dotnet restore "./src/PostService.API/PostService.API.csproj"
COPY . .
WORKDIR "/src/src/PostService.API"
RUN dotnet build "./PostService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PostService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "PostService.API.dll"]