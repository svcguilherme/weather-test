using Microsoft.AspNetCore.Mvc;
using WeatherApi.Services;

namespace WeatherApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController(WeatherService weatherService) : ControllerBase
{
    /// <summary>
    /// Returns the current temperature for a given city.
    /// </summary>
    /// <param name="city">City name (e.g. "São Paulo", "London", "New York")</param>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromQuery] string city)
    {
        if (string.IsNullOrWhiteSpace(city))
            return BadRequest(new { error = "The 'city' query parameter is required." });

        var result = await weatherService.GetWeatherAsync(city);

        if (result is null)
            return NotFound(new { error = $"City '{city}' not found or weather data unavailable." });

        return Ok(result);
    }
}
