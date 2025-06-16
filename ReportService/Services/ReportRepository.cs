using CsvHelper;
using System.Globalization;
using ReportService.Models;

/// <summary>
/// Contains functionality for writing and logging reports 
/// </summary>
public class ReportRepository : IReportRepository
{
    private readonly string _reportPath;

    public ReportRepository(IConfiguration configuration)
    {
        _reportPath = configuration["ReportsDirectory"] ?? "Reports";
        if (!Directory.Exists(_reportPath))
        {
            Directory.CreateDirectory(_reportPath);
        }
    }

    /// <summary>
    /// Writes and logs a report on the imported data
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public async Task WriteReportAsync(ImportResult result)
    {
        var filename = $"report_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv";
        var path = Path.Combine(_reportPath, filename);

        using var writer = new StreamWriter(path);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        csv.WriteHeader<ImportLogEntry>();
        await csv.NextRecordAsync();

        foreach (var success in result.Successes)
        {
            // the import was succesful, error property is empty
            csv.WriteRecord(new ImportLogEntry
            {
                Record = success,
                Status = "Success",
                Error = ""
            });
            await csv.NextRecordAsync();
        }

        foreach (var failure in result.Failures)
        {
            var parts = failure.Split(": ", 2);
            csv.WriteRecord(new ImportLogEntry
            {
                Record = parts[0],
                Status = "Failed",
                Error = parts.Length > 1 ? parts[1] : "Unknown error"
            });
            await csv.NextRecordAsync();
        }
    }
}
