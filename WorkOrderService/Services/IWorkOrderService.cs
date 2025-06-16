public interface IWorkOrderService
{
    Task<ImportResult> ImportWorkOrdersAsync(Stream excelStream);
}