# Stage 1: Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Stage 2: Build and restore dependencies
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file(s) and restore dependencies
COPY ["CMS.csproj", "./"]
RUN dotnet restore

# Copy everything else (including appsettings.json)
COPY . .

# Build the app
RUN dotnet build -c Release -o /app/build

# Stage 3: Publish
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Stage 4: Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# ✅ Ensure appsettings.json is present
COPY appsettings.json .

ENTRYPOINT ["dotnet", "CMS.dll"]
