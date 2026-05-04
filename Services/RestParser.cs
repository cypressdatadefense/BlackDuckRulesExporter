namespace BlackDuckRulesExporter.Services;

using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using BlackDuckRulesExporter.Models;

public static class RestParser
{
    private const int PageSize = 100;
    private static readonly HttpClient Http = new();
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public static async Task<List<Policy>> GetPoliciesAsync(string host, string token)
    {
        host = NormalizeHost(host);

        var jwt = await AuthenticateAsync(host, token);
        Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

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

    private static async Task<string> AuthenticateAsync(string host, string token)
    {
        using var req = new HttpRequestMessage(HttpMethod.Post, $"{host}/api/tokens/authenticate");
        req.Headers.Authorization = new AuthenticationHeaderValue("token", token);
        req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.blackducksoftware.user-4+json"));

        using var resp = await Http.SendAsync(req);
        resp.EnsureSuccessStatusCode();

        var json = await resp.Content.ReadAsStringAsync();
        var auth = JsonSerializer.Deserialize<AuthResponse>(json, JsonOptions)
            ?? throw new InvalidOperationException("Empty response from Black Duck authenticate endpoint.");

        if (string.IsNullOrWhiteSpace(auth.BearerToken))
            throw new InvalidOperationException("Black Duck authenticate response did not include a bearerToken.");

        return auth.BearerToken;
    }

    private static async Task<PoliciesResponse> FetchPageAsync(string host, int offset)
    {
        var url = $"{host}/api/policy-rules?offset={offset}&limit={PageSize}";
        using var req = new HttpRequestMessage(HttpMethod.Get, url);
        req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.blackducksoftware.policy-5+json"));

        using var resp = await Http.SendAsync(req);
        resp.EnsureSuccessStatusCode();

        var json = await resp.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<PoliciesResponse>(json, JsonOptions)
            ?? throw new InvalidOperationException("Empty response from Black Duck API.");
    }

    private static string NormalizeHost(string host) => host.TrimEnd('/');

    private sealed record AuthResponse(
        [property: JsonPropertyName("bearerToken")] string BearerToken,
        [property: JsonPropertyName("expiresInMilliseconds")] long ExpiresInMilliseconds);
}
