FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
# ──────────────────────────────────────────────────────────────
# Stage 1: base – lightweight ASP.NET runtime image used later
# ──────────────────────────────────────────────────────────────

# Use the official .NET 9 ASP.NET runtime image as the base for the final container
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

# Set /app as the working directory inside the container
WORKDIR /app

# Inform Docker that the container listens on port 8080 at runtime
EXPOSE 8080

# ──────────────────────────────────────────────────────────────
# Stage 2: build – full SDK image used to restore, build & publish
# ──────────────────────────────────────────────────────────────

# Use the .NET 9 SDK image (includes compilers & tools) for building the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set /src as the working directory for the build stage
WORKDIR /src

# Copy solution-level build configuration files needed by all projects
COPY Directory.Build.props stylecop.json ./

# Copy only the .csproj files first (one per project) so NuGet restore
# can be cached independently of source code changes
COPY LayeredArchitecture-Task1-Cart-Service/LayeredArchitecture-Task1-Cart-Service.API.csproj LayeredArchitecture-Task1-Cart-Service/
COPY LayeredArchitecture-Task1-Cart-Service.Business/LayeredArchitecture-Task1-Cart-Service.Business.csproj LayeredArchitecture-Task1-Cart-Service.Business/
COPY LayeredArchitecture-Task1-Cart-Service.Repository/LayeredArchitecture-Task1-Cart-Service.Repository.csproj LayeredArchitecture-Task1-Cart-Service.Repository/
COPY LayeredArchitecture-Task1-Cart-Service.Dtos/LayeredArchitecture-Task1-Cart-Service.Dtos.csproj LayeredArchitecture-Task1-Cart-Service.Dtos/
COPY LayeredArchitecture-Task1-CartService.MessageQueue/LayeredArchitecture-Task1-CartService.MessageQueue.csproj LayeredArchitecture-Task1-CartService.MessageQueue/

# Restore NuGet packages for the API project (and its dependencies)
# This layer is cached until a .csproj file changes
RUN dotnet restore LayeredArchitecture-Task1-Cart-Service/LayeredArchitecture-Task1-Cart-Service.API.csproj

# Now copy the full source code (this layer busts the cache on code changes)
COPY . .

# Publish the API project in Release mode to /app/publish
# --no-restore skips restore since it was already done above
RUN dotnet publish LayeredArchitecture-Task1-Cart-Service/LayeredArchitecture-Task1-Cart-Service.API.csproj -c Release -o /app/publish --no-restore

# ──────────────────────────────────────────────────────────────
# Stage 3: final – production-ready image with only the runtime
# ──────────────────────────────────────────────────────────────

# Start from the lightweight base stage (ASP.NET runtime only, no SDK)
FROM base AS final

# Set /app as the working directory
WORKDIR /app

# Copy the published output from the build stage into this image
COPY --from=build /app/publish .

# Define the command that runs when the container starts
ENTRYPOINT ["dotnet", "LayeredArchitecture-Task1-Cart-Service.API.dll"]
