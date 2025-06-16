/// <summary>
/// A reference to the class that contains 
/// functionality for writing and logging reports 
/// </summary>
public interface IReportRepository
{
    /// <summary>
    /// Reference to an asynchronous method for writing reports
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    Task WriteReportAsync(ImportResult result);
}