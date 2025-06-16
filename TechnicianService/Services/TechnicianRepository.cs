using OfficeOpenXml;

public class TechnicianRepository : ITechnicianRepository
{
    private readonly TechnicianDbContext _db;
    private readonly IHttpClientFactory _httpClientFactory;

    public TechnicianRepository(TechnicianDbContext db, IHttpClientFactory httpClientFactory)
    {
        _db = db;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<ImportResult> ImportTechniciansAsync(Stream excelStream)
    {
        ExcelPackage.License.SetNonCommercialPersonal("Bruno Baci");

        using var package = new ExcelPackage(excelStream);
        var worksheet = package.Workbook.Worksheets[0];
        var rowCount = worksheet.Dimension.Rows;

        var result = new ImportResult();
        var namesSet = new HashSet<string>();

        for (int row = 2; row <= rowCount; row++)
        {
            string fullName = worksheet.Cells[row, 1].Text;
            if (string.IsNullOrWhiteSpace(fullName) || namesSet.Contains(fullName)) continue;

            namesSet.Add(fullName);
            var parts = fullName.Split(' ', 2);
            var firstName = parts.Length > 0 ? parts[0] : "";
            var lastName = parts.Length > 1 ? parts[1] : "";

            try
            {
                _db.Technicians.Add(new Technician { FirstName = firstName, LastName = lastName });
                result.Successes.Add(fullName);
            }
            catch (Exception ex)
            {
                result.Failures.Add($"{fullName}: {ex.Message}");
            }
        }

        await _db.SaveChangesAsync();
        await SendReportToService(result);
        return result;
    }

    private async Task SendReportToService(ImportResult result)
    {
        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri("http://reportservice");

        try
        {
            await client.PostAsJsonAsync("/api/report/log", result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send report: {ex.Message}");
        }
    }
}