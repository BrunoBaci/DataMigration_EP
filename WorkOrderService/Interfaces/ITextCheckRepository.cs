public interface ITextCheckRepository
{
    Task<(string?, string?)> MatchAsync(List<string> name);
    Task LoadListToCheckAsync(List<string> allnames);
}