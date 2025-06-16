using OfficeOpenXml;
using System.Net.Http.Json;

public class ClientImportService : IClientRepository
{
    private readonly ClientDbContext _db;
    private readonly IHttpClientFactory _httpClientFactory;

    public ClientImportService(ClientDbContext db, IHttpClientFactory httpClientFactory)
    {
        _db = db;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<ImportResult> ImportClientsAsync(Stream excelStream)
    {
        ExcelPackage.License.SetNonCommercialPersonal("Bruno Baci");
        using var package = new ExcelPackage(excelStream);
        var worksheet = package.Workbook.Worksheets[0];
        var rowCount = worksheet.Dimension.Rows;

        var result = new ImportResult();

        for (int row = 2; row <= rowCount; row++)
        {
            var fullName = worksheet.Cells[row, 1].Text;
            if (string.IsNullOrEmpty(fullName))
            {
                continue;
            }
            var nameParts = fullName.Split(' ', 2);
            var firstName = nameParts[0];
            var lastName = nameParts[1]; ;

            try
            {
                _db.Clients.Add(new Client { FirstName = firstName, LastName = lastName });
                result.Successes.Add($"{firstName} {lastName}");
            }
            catch (Exception ex)
            {
                result.Failures.Add($"{firstName} {lastName}: {ex.Message}");
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