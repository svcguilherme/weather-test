namespace WeatherApi.Models;

public record WeatherResponse(
    string City,
    string Country,
    double Latitude,
    double Longitude,
    double TemperatureCelsius,
    double TemperatureFahrenheit,
    string LastUpdated
);

// Open-Meteo geocoding response
public class GeocodingResult
{
    public List<GeocodingLocation>? Results { get; set; }
}

public class GeocodingLocation
{
    public string Name { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Country { get; set; } = string.Empty;
    public string? Admin1 { get; set; }
}

// Open-Meteo forecast response
public class ForecastResult
{
    public CurrentWeather? Current { get; set; }
}

public class CurrentWeather
{
    public string? Time { get; set; }
    public double Temperature_2m { get; set; }
}
