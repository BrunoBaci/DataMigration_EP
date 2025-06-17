using System.Text.RegularExpressions;

namespace WorkOrderService.Interfaces
{
    public interface IParser
    {
        public List<string> ExtractClientName(string notes);

        public DateTime ExtractDate(string notes);

        public string RemoveClientFromNotes(string notes, string name, DateTime date);
    }
}
