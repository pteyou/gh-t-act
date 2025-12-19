using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace TestApi.Tests;

public class WeatherForecastTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public WeatherForecastTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetWeatherForecast_ReturnsFiveForecasts()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/weatherforecast");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var doc = JsonSerializer.Deserialize<List<WeatherForecast>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.NotNull(doc);
        Assert.Equal(5, doc.Count);

        foreach (var forecast in doc)
        {
            Assert.InRange(forecast.TemperatureC, -20, 55);
            Assert.False(string.IsNullOrEmpty(forecast.Summary));
        }
    }
}
