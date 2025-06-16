public interface ITechnicianRepository
{
    Task<ImportResult> ImportTechniciansAsync(Stream excelStream);
}