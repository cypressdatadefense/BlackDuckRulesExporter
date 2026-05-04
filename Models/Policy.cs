namespace BlackDuckRulesExporter.Models;

public class Policy
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Severity { get; set; } = "";
    public bool Enabled { get; set; }
}

public class PoliciesResponse
{
    public int TotalCount { get; set; }
    public List<Policy> Items { get; set; } = [];
}
