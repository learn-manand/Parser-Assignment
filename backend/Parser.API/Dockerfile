# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project file
COPY Parser.API/Parser.API.csproj Parser.API/

# Restore
RUN dotnet restore Parser.API/Parser.API.csproj

# Copy full source
COPY . .

WORKDIR /src/Parser.API
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "Parser.API.dll"]