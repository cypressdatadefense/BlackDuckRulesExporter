namespace BlackDuckRulesExporter.Services;

using ClosedXML.Excel;
using BlackDuckRulesExporter.Models;

public static class ExcelService
{
    private static readonly XLColor HeaderBg = XLColor.FromArgb(32, 55, 100);
    private static readonly string[] Headers = ["#", "Policy Name", "Severity", "Enabled", "Description"];

    public static void Export(List<Policy> policies, string fileName)
    {
        using var workbook = new XLWorkbook();
        var sheet = workbook.Worksheets.Add("Policies");
        WriteHeader(sheet);
        WriteData(sheet, policies);
        for (int col = 1; col <= Headers.Length; col++)
            sheet.Column(col).AdjustToContents();
        workbook.SaveAs(fileName);
    }

    private static void WriteHeader(IXLWorksheet sheet)
    {
        for (int i = 0; i < Headers.Length; i++)
        {
            var cell = sheet.Cell(1, i + 1);
            cell.Value = Headers[i];
            cell.Style.Fill.BackgroundColor = HeaderBg;
            cell.Style.Font.Bold = true;
            cell.Style.Font.FontSize = 12;
            cell.Style.Font.FontColor = XLColor.White;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cell.Style.Alignment.WrapText = true;
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
        }
    }

    private static void WriteData(IXLWorksheet sheet, List<Policy> policies)
    {
        for (int i = 0; i < policies.Count; i++)
        {
            var policy = policies[i];
            int row = i + 2;

            sheet.Cell(row, 1).Value = i + 1;
            sheet.Cell(row, 2).Value = policy.Name;
            sheet.Cell(row, 3).Value = policy.Severity;
            sheet.Cell(row, 4).Value = policy.Enabled ? "Yes" : "No";
            sheet.Cell(row, 5).Value = policy.Description;

            for (int col = 1; col <= Headers.Length; col++)
            {
                var cell = sheet.Cell(row, col);
                cell.Style.Fill.BackgroundColor = XLColor.White;
                cell.Style.Font.FontSize = 11;
                cell.Style.Font.FontColor = XLColor.Black;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                cell.Style.Alignment.WrapText = true;
                cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            }
        }
    }
}
