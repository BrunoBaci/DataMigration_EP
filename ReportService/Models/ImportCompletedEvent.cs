namespace ReportService.Models
{
    public class ImportCompletedEvent
    {
        public string ImportType { get; set; }
        public DateTime Timestamp { get; set; }
        public List<string> SuccessRows { get; set; }
        public List<string> FailedRows { get; set; }
    }
}
