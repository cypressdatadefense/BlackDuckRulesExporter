using BlackDuckRulesExporter.Services;

var asJson = args.Contains("--json");
var positional = args.Where(a => a != "--json").ToArray();

if (positional.Length < 2 || string.IsNullOrWhiteSpace(positional[0]) || string.IsNullOrWhiteSpace(positional[1]))
{
    Console.Error.WriteLine("Usage: BlackDuckRulesExporter <blackduck-url> <api-token> [--json]");
    return 1;
}

var url = positional[0];
var token = positional[1];

Console.WriteLine($"Fetching policies from {url}...");
var policies = await RestParser.GetPoliciesAsync(url, token);
Console.WriteLine($"Fetched {policies.Count} policies.");

var fileName = asJson ? "BD_POLICIES.json" : "BD_POLICIES.xlsx";

if (asJson)
    JsonService.Export(policies, fileName);
else
    ExcelService.Export(policies, fileName);

Console.WriteLine($"Generated File - {fileName}");
return 0;
