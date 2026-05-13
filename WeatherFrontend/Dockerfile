# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY WeatherFrontend.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish WeatherFrontend.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5051
EXPOSE 5051

ENTRYPOINT ["dotnet", "WeatherFrontend.dll"]
