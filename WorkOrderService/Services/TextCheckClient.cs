using System.Net.Http.Json;

public class TextCheckClient : ITextCheckClient
{
    private readonly HttpClient _http;

    public TextCheckClient(IHttpClientFactory factory)
    {
        _http = factory.CreateClient();
        // temmporary value for testing
        _http.BaseAddress = new Uri("https://localhost:61860");
    }
    public async Task LoadListToCheckAsync(List<string> candidates)
    {
        await _http.PostAsJsonAsync("/api/fuzzymatch/load-candidates", candidates);
    }
    public async Task<MatchTextResult?> MatchAsync(string inputName)
    {
        var response = await _http.GetAsync($"/api/fuzzymatch/match?input={Uri.EscapeDataString(inputName)}");
        return await response.Content.ReadFromJsonAsync<MatchTextResult>();
    }
}