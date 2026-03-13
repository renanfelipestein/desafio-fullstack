using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

public class ClimaService
{
    private readonly AppDbContext _context;
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    private const string BaseUrl = "https://api.openweathermap.org/data/2.5/weather";

    public ClimaService(HttpClient httpClient)    {

        _httpClient = httpClient;
        _apiKey = Environment.GetEnvironmentVariable("API_KEY_OPENWEATHER");            
    }

public async Task<ConsultaCidadeDTO> ConsultarPorCidadeAsync(string cidade)
    {
        var url = $"{BaseUrl}?q={cidade}&appid={_apiKey}&units=metric&lang=pt_br";
        var dados = await ExecutarConsultaAsync(url);

        return new ConsultaCidadeDTO
        {
            Cidade      = dados.Cidade,
            Temperatura = dados.Main.Temp
        };
    }

    public async Task<ConsultaLatLogDTO> ConsultarPorCoordenadasAsync(double lat, double lon)
    {
        var url = $"{BaseUrl}?lat={lat}&lon={lon}&appid={_apiKey}&units=metric&lang=pt_br";
        var dados = await ExecutarConsultaAsync(url);

        return new ConsultaLatLogDTO
        {
            Latitude    = dados.Coord.Lat,
            Longitude   = dados.Coord.Lon,
            Temperatura = dados.Main.Temp
        };
    }

    private async Task<OpenWeatherResponse> ExecutarConsultaAsync(string url)
    {
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Erro ao consultar OpenWeatherMap: {response.StatusCode}");

        var content = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<OpenWeatherResponse>(content)
            ?? throw new Exception("Resposta inválida da API de clima.");
    }
}

internal class OpenWeatherResponse
{
    [JsonPropertyName("name")]
    public string Cidade { get; set; } = string.Empty;

    [JsonPropertyName("coord")]
    public CoordResponse Coord { get; set; } = new();

    [JsonPropertyName("main")]
    public MainResponse Main { get; set; } = new();
}

internal class CoordResponse
{
    [JsonPropertyName("lat")]
    public double Lat { get; set; }

    [JsonPropertyName("lon")]
    public double Lon { get; set; }
}

internal class MainResponse
{
    [JsonPropertyName("temp")]
    public decimal Temp { get; set; }
}