public interface IWorkOrderRepository
{
    Task<ImportResult> ImportWorkOrdersAsync(Stream excelStream);
}