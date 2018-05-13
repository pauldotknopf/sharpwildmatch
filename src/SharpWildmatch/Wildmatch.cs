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
        // ReSharper restore InconsistentNaming
        
        [Flags]
        enum MatchFlags
        {
            None = 0,
            CaseFolder = 1,
            PathName = 2
        }
        

        private static int DoWild(string pattern, string text, MatchFlags flags)
        {
            var patternIndex = 0;
            var textIndex = 0;
            for (; patternIndex < pattern.Length; patternIndex++, textIndex++)
            {
                bool matchSlash;
                int matched;
                int negated;
                var textChar = text.At(textIndex);
                var patternChar = pattern.At(patternIndex);
                char? patternCharPrevious = null;
                
                if (textChar == null && patternChar != '*')
                    return WM_ABORT_ALL;

                switch (patternChar)
                {
                    case '\\':
                        patternIndex++;
                        patternChar = pattern.At(patternIndex);
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
                        patternChar = pattern.At(patternIndex);
                        if (pattern.At(patternIndex) == '*')
                        {
                            // TODO:
                            return WM_ABORT_MALFORMED;
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

                        while (true)
                        {
                            if(textChar == null)
                                break;

                            if (!Sane.IsGlobSpecial(patternChar.Value)) {
                                while (textChar != null && (matchSlash || textChar != '/'))
                                {
                                    if(textChar == patternChar)
                                        break;
                                    textIndex++;
                                    textChar = text.At(textIndex);
                                }
                                if (textChar != patternChar)
                                    return WM_NOMATCH;
                            }
                            
                            if ((matched = DoWild(pattern.Substring(patternIndex), text.Substring(textIndex), flags)) != WM_NOMATCH) {
                                if (!matchSlash || matched != WM_ABORT_TO_STARSTAR)
                                    return matched;
                            } else if (!matchSlash && textChar == '/')
                                return WM_ABORT_TO_STARSTAR;

                            textIndex++;
                            textChar = text.At(textIndex);
                        }
                        
                        return WM_ABORT_ALL;
                    case '[':
                        patternIndex++;
                        patternChar = pattern.At(patternIndex);
                        
                        negated = patternChar == '!' ? 1 : 0;
                        if (negated == 1)
                        {
                            patternIndex++;
                            patternChar = pattern.At(patternIndex);
                        }
                        
                        patternCharPrevious = null;
                        matched = 0;

                        bool Next()
                        {
                            patternCharPrevious = patternChar;
                            patternIndex++;
                            patternChar = pattern.At(patternIndex);
                            return patternChar != ']';
                        }

                        do
                        {
                            if (false)
                            {
                                
                            }else if (false)
                            {
                                
                            }else if (false)
                            {
                                
                            }else if (patternChar == textChar)
                            {
                                matched = 1;
                            }
                        } while (Next());
                        
                        if (matched == negated ||
                            (flags.HasFlag(MatchFlags.PathName)) && textChar == '/')
                            return WM_NOMATCH;
                        continue;
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