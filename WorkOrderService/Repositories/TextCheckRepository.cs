using System.Net.Http.Json;
using Fastenshtein;
using Microsoft.EntityFrameworkCore.Storage.Internal;
public class TextCheckRepository : ITextCheckRepository
{
    private readonly HttpClient _http;
    private List<string> _allnames;
    public TextCheckRepository()
    {
    }
    public async Task LoadListToCheckAsync(List<string> allnames)
    {
        _allnames = allnames;
    }

    public async Task<(string?, string?)> MatchAsync(List<string> names)
    {
        string actualName = "";
        string nameToRemove = "";
        foreach (string name in names)
        {
            var r = _allnames
                .Select(n => new
                    {
                        Value = n,
                        Dist = Levenshtein.Distance(name, n)
                    })
                .OrderBy(x => x.Dist).FirstOrDefault();
        
            int levenShteinDistance = r?.Dist ?? -1; 
            
            if (levenShteinDistance <= 2)
            {
                actualName = r.Value;
                nameToRemove = name;
            }
            else
            {
                continue;
            }
        }
        return (actualName, nameToRemove);
    }
}