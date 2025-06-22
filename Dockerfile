# Multi-stage Dockerfile for Blazor Server Application
# Stage 1: Build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file and project files for better layer caching
COPY ["src/JCP.BeerContest/BeerContest.sln", "src/JCP.BeerContest/"]
COPY ["src/JCP.BeerContest/BeerContest.Web/BeerContest.Web.csproj", "src/JCP.BeerContest/BeerContest.Web/"]
COPY ["src/JCP.BeerContest/BeerContest.Application/BeerContest.Application.csproj", "src/JCP.BeerContest/BeerContest.Application/"]
COPY ["src/JCP.BeerContest/BeerContest.Infrastructure/BeerContest.Infrastructure.csproj", "src/JCP.BeerContest/BeerContest.Infrastructure/"]
COPY ["src/JCP.BeerContest/BeerContest.Domain/BeerContest.Domain.csproj", "src/JCP.BeerContest/BeerContest.Domain/"]

# Restore dependencies
RUN dotnet restore "src/JCP.BeerContest/BeerContest.Web/BeerContest.Web.csproj"

# Copy the entire source code
COPY . .

# Build the application
WORKDIR "/src/src/JCP.BeerContest/BeerContest.Web"
RUN dotnet build "BeerContest.Web.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "BeerContest.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Create a non-root user for security
RUN addgroup --system --gid 1001 blazorgroup && \
    adduser --system --uid 1001 --ingroup blazorgroup blazoruser

# Copy the published application
COPY --from=publish /app/publish .

# Set proper ownership
RUN chown -R blazoruser:blazorgroup /app

# Switch to non-root user
USER blazoruser

# Expose the port
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

# Start the application
ENTRYPOINT ["dotnet", "BeerContest.Web.dll"]
