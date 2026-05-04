namespace BlackDuckRulesExporter.Services;

using System.Text.Json;
using BlackDuckRulesExporter.Models;

public static class JsonService
{
    private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };

    public static void Export(List<Policy> policies, string fileName)
    {
        var json = JsonSerializer.Serialize(policies, Options);
        File.WriteAllText(fileName, json);
    }
}
