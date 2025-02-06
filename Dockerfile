# Use the .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the solution file
COPY *.sln .

# Copy project files for the main project and class libraries
COPY src/IdentityService/*.csproj ./src/IdentityService/
COPY src/IdentityService.Services/*.csproj ./src/IdentityService.Services/
COPY src/IdentityService.Model/*.csproj ./src/IdentityService.Model/
COPY src/IdentityService.DataStorage/*.csproj ./src/IdentityService.DataStorage/

# Restore dependencies
RUN dotnet restore

# Copy the remaining files for all projects
COPY . .

# Build the project
WORKDIR /app/src/IdentityService
RUN dotnet publish -c Release -o /out

# Use the .NET runtime image for the final application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory inside the container
WORKDIR /app

# Copy the built application from the build stage
COPY --from=build /out .

# Expose the application port (default for ASP.NET Core apps is 80)
EXPOSE 8080

# Set the entry point
ENTRYPOINT ["dotnet", "IdentityService.dll"]
