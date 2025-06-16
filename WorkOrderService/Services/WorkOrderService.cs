using OfficeOpenXml;
using System.Linq;
using System.Net.Http.Json;

public class WorkOrderService : IWorkOrderService
{
    private readonly WorkOrderDbContext _db;
    private readonly ITextCheckClient _textChecker;

    public WorkOrderService(WorkOrderDbContext db, ITextCheckClient textChecker)
    {
        _db = db;
        _textChecker = textChecker;
    }

    public async Task<ImportResult> ImportWorkOrdersAsync(Stream excelStream)
    {
        ExcelPackage.License.SetNonCommercialPersonal("Bruno Baci");

        using var package = new ExcelPackage(excelStream);
        var worksheet = package.Workbook.Worksheets[0];
        var rowCount = worksheet.Dimension.Rows;

        var result = new ImportResult();
        var clientList = _db.Clients.Select(c => c.FullName).ToList();
        await _textChecker.LoadListToCheckAsync(clientList);
        
        for (int row = 2; row <= rowCount; row++)
        {
            var technicianName = worksheet.Cells[row, 1].Text;
            var technician = _db.Technicians.Where(t => t.FullName == technicianName).ToList().FirstOrDefault();


            var notes = worksheet.Cells[row, 2].Text;
            var total = decimal.TryParse(worksheet.Cells[row, 3].Text, out var t) ? t : 0;

            var name = WorkOrderParser.ExtractClientName(notes);

            var date = WorkOrderParser.ExtractDate(notes);

            var match = await _textChecker.MatchAsync(name);

            var matchedClient = _db.Clients.FirstOrDefault(c => c.FullName == match.MatchedName);

            if (matchedClient == null)
            {
                result.Failures.Add($"{name}: No matching client found");
                continue;
            }

            var cleanInfo = WorkOrderParser.RemoveClientFromNotes(notes, name, date);

            _db.WorkOrders.Add(new WorkOrder
            {
                ClientId = matchedClient.Id,
                TechnicianId = technician.Id,
                Information = cleanInfo,
                Date = date,
                Total = total
            });

            result.Successes.Add($"{technician} → {matchedClient.FullName}");
        }

        await _db.SaveChangesAsync();
        return result;
    }
}