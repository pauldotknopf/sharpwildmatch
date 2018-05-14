namespace SharpWildmatch
{
    public enum MatchResult: int
    {
        AbortAll = -1,
        AbortToStartStart = -2,
        Match = 0,
        NoMatch = 1,
        AbortMalformed = 2
    }
}