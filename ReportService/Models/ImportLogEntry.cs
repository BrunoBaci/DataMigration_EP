namespace ReportService.Models
{
    /// <summary>
    /// Contains log entry info
    /// </summary>
    public class ImportLogEntry
    {
        /// <summary>
        /// The status being logged
        /// </summary>
        public string Record { get; set; }

        /// <summary>
        /// The status of the record
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// The error being logged
        /// </summary>
        public string Error { get; set; }
    }
}
