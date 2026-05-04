namespace BlackDuckRulesExporter.Services;

using System.Net.Http.Headers;
using System.Text.Json;
using BlackDuckRulesExporter.Models;

public static class RestParser
{
    private const int PageSize = 100;
    private static readonly HttpClient Http = new();
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public static async Task<List<Policy>> GetPoliciesAsync(string host, string token)
    {
        host = NormalizeHost(host);
        Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var first = await FetchPageAsync(host, 0);
        var policies = new List<Policy>(first.Items);

        int offset = PageSize;
        while (offset < first.TotalCount)
        {
            var resp = await FetchPageAsync(host, offset);
            policies.AddRange(resp.Items);
            offset += PageSize;
        }

        return policies;
    }

    private static async Task<PoliciesResponse> FetchPageAsync(string host, int offset)
    {
        var url = $"{host}/api/policies?offset={offset}&limit={PageSize}";
        var json = await Http.GetStringAsync(url);
        return JsonSerializer.Deserialize<PoliciesResponse>(json, JsonOptions)
            ?? throw new InvalidOperationException("Empty response from Black Duck API.");
    }

    private static string NormalizeHost(string host) => host.TrimEnd('/');
}
