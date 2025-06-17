using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Linq;
using System.Net.Http.Json;
using WorkOrderService.Interfaces;
using WorkOrderService.Repositories;

namespace WorkOrderService.Services
{
    public class WorkOrderRepository : IWorkOrderRepository
    {
        private readonly WorkOrderDbContext _db;
        private readonly ITextCheckRepository _textChecker;
        private readonly IParser _workOrderParser;
        private readonly IReportClient _reportClient;

        public WorkOrderRepository(WorkOrderDbContext db, ITextCheckRepository textChecker, IParser workOrderParser, IReportClient reportClient)
        {
            _db = db;
            _textChecker = textChecker;
            _workOrderParser = workOrderParser;
            _reportClient = reportClient;
        }

        public async Task<ImportResult> ImportWorkOrdersAsync(Stream excelStream)
        {
            ExcelPackage.License.SetNonCommercialPersonal("Bruno Baci");

            using var package = new ExcelPackage(excelStream);
            var worksheet = package.Workbook.Worksheets[0];
            var rowCount = worksheet.Dimension.Rows;

            var result = new ImportResult();

            var clients = await _db.Clients.Select(c => new { c.Id, FullName = c.FirstName + " " + c.LastName }).ToListAsync();

            var clientDict = clients.ToDictionary(c => c.FullName, c => c.Id);

            var technicians = await _db.Technicians
                .Select(t => new { t.Id, FullName = t.FirstName + " " + t.LastName })
                .ToListAsync();

            var technicianDict = technicians.ToDictionary(t => t.FullName, t => t.Id);
            await _textChecker.LoadListToCheckAsync(clientDict.Keys.ToList());

            for (int row = 2; row <= rowCount; row++)
            {
                var technicianName = worksheet.Cells[row, 1].Text;

                var notes = worksheet.Cells[row, 2].Text;
                if (!technicianDict.TryGetValue(technicianName, out var technicianId))
                {
                    result.Failures.Add($"Row: {row}, Technician not found: {technicianName}");
                    continue;
                }

                var total = decimal.TryParse(worksheet.Cells[row, 3].Text, out var t) ? t : 0;

                var rawClientName = _workOrderParser.ExtractClientName(notes);
                var date = _workOrderParser.ExtractDate(notes);

                var (matchedName, matchedAlias) = await _textChecker.MatchAsync(rawClientName);

                if (matchedName == null || !clientDict.TryGetValue(matchedName, out var clientId))
                {
                    result.Failures.Add($"Row: {row}, Technician: {technicianName}, No matching client for extracted '{rawClientName}'");
                    continue;
                }

                var cleanInfo = _workOrderParser.RemoveClientFromNotes(notes, matchedAlias, date);

                _db.WorkOrders.Add(new WorkOrder
                {
                    ClientId = clientId,
                    TechnicianId = technicianId,
                    Information = cleanInfo,
                    Date = date,
                    Total = total
                });

                result.Successes.Add($"Row: {row}, Technician: {technicianName}, Client: {matchedName}");
            }


            await _db.SaveChangesAsync();
            await _reportClient.CreateCSV(result);
            return result;
        }
    }
}