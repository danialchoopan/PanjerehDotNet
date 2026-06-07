# Multi-stage build for PanjerehDotNet
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copy solution and project files for restore
COPY PanjerehDotNet.slnx ./
COPY PanjerehDotNet/src/PanjerehDotNet.Domain/*.csproj ./PanjerehDotNet/src/PanjerehDotNet.Domain/
COPY PanjerehDotNet/src/PanjerehDotNet.Application/*.csproj ./PanjerehDotNet/src/PanjerehDotNet.Application/
COPY PanjerehDotNet/src/PanjerehDotNet.Infrastructure/*.csproj ./PanjerehDotNet/src/PanjerehDotNet.Infrastructure/
COPY PanjerehDotNet/src/PanjerehDotNet.Web/*.csproj ./PanjerehDotNet/src/PanjerehDotNet.Web/

# Restore dependencies
RUN dotnet restore PanjerehDotNet/src/PanjerehDotNet.Web/PanjerehDotNet.Web.csproj

# Copy everything else and build
COPY . .
WORKDIR /app/PanjerehDotNet/src/PanjerehDotNet.Web
RUN dotnet publish -c Release -o /app/out

# Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/out .

# Create uploads directory
RUN mkdir -p wwwroot/uploads

ENTRYPOINT ["dotnet", "PanjerehDotNet.Web.dll"]
