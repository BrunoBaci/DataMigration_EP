public interface ITextCheckClient
{
    Task<MatchTextResult?> MatchAsync(string inputName);
    Task LoadListToCheckAsync(List<string> candidates);
}