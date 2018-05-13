using System;
using System.Diagnostics;

namespace SharpWildmatch
{
    public class Wildmatch
    {
        // ReSharper disable InconsistentNaming
        public const int WM_ABORT_MALFORMED = 2;
        public const int WM_NOMATCH = 1;
        public const int WM_MATCH = 0;
        public const int WM_ABORT_ALL = -1;
        public const int WM_ABORT_TO_STARSTAR = -2;

        [Flags]
        enum MatchFlags
        {
            None = 0,
            CaseFolder = 1,
            PathName = 2
        }
        // ReSharper restore InconsistentNaming

        private static int DoWild(string pattern, string text, MatchFlags flags)
        {
            var patternIndex = 0;
            var textIndex = 0;
            for (; patternIndex < pattern.Length; patternIndex++, textIndex++)
            {
                bool matchSlash;
                var textChar = text.At(textIndex);
                var patternChar = pattern.At(patternIndex);
                
                if (textChar == null && patternChar != '*')
                    return WM_ABORT_ALL;

                switch (patternChar)
                {
                    case '\\':
                        patternIndex++;
                        // fallthrough
                        goto default;
                    default:
                        if (textChar != patternChar)
                            return WM_NOMATCH;
                        continue;
                    case '?':
                        if (flags.HasFlag(MatchFlags.CaseFolder) && textChar == '/')
                            return WM_NOMATCH;
                        continue;
                    case '*':
                        patternIndex++;
                        if (pattern.At(patternIndex) == '*')
                        {
                            var patterCharPrevious = pattern.At(patternIndex - 2);
                            while (patternChar == '*')
                            {
                                patternIndex++;
                                patternChar = pattern.At(patternIndex);
                            }

                            if (!flags.HasFlag(MatchFlags.PathName))
                            {
                                matchSlash = true;
                            }
                            else if (true) // TODO
                            {
                                return WM_ABORT_MALFORMED;
                            }
                            else
                            {
                                return WM_ABORT_MALFORMED;
                            }
                        }
                        else
                        {
                            matchSlash = !flags.HasFlag(MatchFlags.PathName);
                        }
                        
                        if (patternChar == null) {
                            /* Trailing "**" matches everything.  Trailing "*" matches
                             * only if there are no more slash characters. */
                            if (!matchSlash)
                            {
                                if (text.Contains("/"))
                                    return WM_NOMATCH;
                            }
                            return WM_MATCH;
                        } else if (!matchSlash && patternChar == '/') {
                            // TODO:
                            return WM_ABORT_MALFORMED;
                        }
                        
                        return WM_ABORT_ALL;
                }
            }

            return textIndex < text.Length ? WM_NOMATCH : WM_MATCH;
        }
        
        public static bool Match(string pattern, string text, uint flags)
        {
            return DoWild(pattern, text, MatchFlags.None) == 0;
        }
    }
}