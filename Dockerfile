# Stage 1: Base runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Stage 2: SDK for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
# This layer is cached unless your dependencies change
COPY ["PortfolioShop/PortfolioShop.csproj", "PortfolioShop/"]
COPY ["Portfolio.Core/Portfolio.Core.csproj", "Portfolio.Core/"]
RUN dotnet restore "PortfolioShop/PortfolioShop.csproj"

# Copy the rest of the source code
COPY . .
WORKDIR "/src/PortfolioShop"
RUN dotnet build "PortfolioShop.csproj" -c Release -o /app/build

# Stage 3: Publish the app
FROM build AS publish
RUN dotnet publish "PortfolioShop.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 4: Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PortfolioShop.dll"]