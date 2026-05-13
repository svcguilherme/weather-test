using System.Text.Json;
using WeatherApi.Models;

namespace WeatherApi.Services;

public class WeatherService(IHttpClientFactory httpClientFactory, ILogger<WeatherService> logger)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<WeatherResponse?> GetWeatherAsync(string city)
    {
        var http = httpClientFactory.CreateClient();

        // Step 1: Geocode city name → lat/lon using Open-Meteo geocoding API
        var geoUrl = $"https://geocoding-api.open-meteo.com/v1/search?name={Uri.EscapeDataString(city)}&count=1&language=pt";
        GeocodingResult? geo;
        try
        {
            var geoJson = await http.GetStringAsync(geoUrl);
            geo = JsonSerializer.Deserialize<GeocodingResult>(geoJson, JsonOptions);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to geocode city {City}", city);
            return null;
        }

        var location = geo?.Results?.FirstOrDefault();
        if (location is null)
        {
            logger.LogWarning("City not found: {City}", city);
            return null;
        }

        // Step 2: Fetch current temperature from Open-Meteo forecast API
        var forecastUrl = $"https://api.open-meteo.com/v1/forecast" +
                          $"?latitude={location.Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                          $"&longitude={location.Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                          $"&current=temperature_2m" +
                          $"&temperature_unit=celsius";

        ForecastResult? forecast;
        try
        {
            var forecastJson = await http.GetStringAsync(forecastUrl);
            forecast = JsonSerializer.Deserialize<ForecastResult>(forecastJson, JsonOptions);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch forecast for {City}", city);
            return null;
        }

        var tempC = forecast?.Current?.Temperature_2m ?? 0;
        var tempF = Math.Round(tempC * 9.0 / 5.0 + 32, 1);

        return new WeatherResponse(
            City: location.Name,
            Country: location.Country,
            Latitude: location.Latitude,
            Longitude: location.Longitude,
            TemperatureCelsius: tempC,
            TemperatureFahrenheit: tempF,
            LastUpdated: forecast?.Current?.Time ?? DateTime.UtcNow.ToString("o")
        );
    }
}
