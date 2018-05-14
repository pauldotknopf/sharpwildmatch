using System;

namespace SharpWildmatch
{
    [Flags]
    public enum MatchFlags
    {
        None = 0,
        CaseFold = 1,
        PathName = 2
    }
}