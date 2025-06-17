namespace WorkOrderService.Interfaces
{
    public interface IReportClient
    {
        Task<HttpResponseMessage> CreateCSV(ImportResult importResult);
    }
}
