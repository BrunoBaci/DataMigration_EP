using System.Text.RegularExpressions;

public static class WorkOrderParser
{
    public static string ExtractClientName(string notes)
    {
        var match = Regex.Match(notes, @"klienti\s+(\w+\s\w+)", RegexOptions.IgnoreCase);
        return match.Success ? match.Groups[1].Value : string.Empty;
    }

    public static DateTime ExtractDate(string notes)
    {
        var match = Regex.Match(notes, @"daten\s+(\d{2}/\d{2}/\d{4})", RegexOptions.IgnoreCase);
        return match.Success ? DateTime.Parse(match.Groups[1].Value) : DateTime.MinValue;
    }

    public static string RemoveClientFromNotes(string notes, string name, DateTime date)
    {
        return notes.Replace(name, "").Replace(date.ToShortDateString(), "").Trim();
    }
}