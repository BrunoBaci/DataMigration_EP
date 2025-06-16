public interface IClientRepository
{
    Task<ImportResult> ImportClientsAsync(Stream excelStream);
}