# Use the official .NET SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY . ./

RUN dotnet restore ./src/GenericApi/GenericApi.csproj

# Copy everything else and build
RUN dotnet publish ./src/GenericApi/GenericApi.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .


# Start the application
ENTRYPOINT ["dotnet", "GenericApi.dll"]
