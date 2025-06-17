using System.Globalization;
using System.Text.RegularExpressions;
using WorkOrderService.Interfaces;
using static OfficeOpenXml.ExcelErrorValue;

/// <summary>
/// Contains functions for extracting data
/// </summary>
public class Parser : IParser
{
    public List<string> ExtractClientName(string notes)
    {
        // can't cover all cases so we will return more than one match then check if any of those matches with the client name using the Levenshtein algorithm
        var matches = Regex.Matches(notes, @"\b(?:te\s+klienti|te|klienti)\s+([A-Z][a-z]+\s[A-Z][a-z]+)\b", RegexOptions.IgnoreCase);

        if (matches.Count > 0)
        {
            return matches.Select(x => x.Groups[1].Value.Trim()).ToList();
        }
        else
            return new List<string>();
    }

    public DateTime ExtractDate(string notes)
    {
        var match = Regex.Match(notes, @"\b(\d{1,2}/\d{1,2}/\d{4})\b", RegexOptions.IgnoreCase);

        if (match.Success)
        {
            var dateString = match.Groups[1].Value;

            if (DateTime.TryParseExact(dateString, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result) ||
                DateTime.TryParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                return result;
            }
        }
        return DateTime.MinValue;
    }

    public string RemoveClientFromNotes(string notes, string name, DateTime date)
    {       

        if (!string.IsNullOrWhiteSpace(name))
            notes = Regex.Replace(notes, @$"\b(?:te|te klienti)\s+{Regex.Escape(name)}\b", "", RegexOptions.IgnoreCase);
        

        if (date != DateTime.MinValue)
        {
            // doesn't work for all cases, such as when
            string? datePattern = date.ToString("d/M/yyyy").Replace("-", "/");
            notes = Regex.Replace(notes, @$"[\s,;:]*((me\s+)?(data|daten|me daten)\s+)?{Regex.Escape(datePattern)}[.,;]*", "",RegexOptions.IgnoreCase).Trim();
        }

        return notes;
    }
}