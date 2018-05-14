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
        
        private static int DoWild(string pattern, string text, MatchFlags flags)
        {
            var patternIndex = 0;
            var textIndex = 0;
            for (; patternIndex < pattern.Length; patternIndex++, textIndex++)
            {
                bool matchSlash = false;
                int matched = 0;
                int negated = 0;
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
                        if (flags.HasFlag(MatchFlags.CaseFold) && textChar == '/')
                            return WM_NOMATCH;
                        continue;
                    case '*':
                        patternIndex++;
                        patternChar = pattern.At(patternIndex);
                        if (pattern.At(patternIndex) == '*')
                        {
                            var previous = pattern.At(patternIndex - 2);
                            
                            patternIndex++;
                            patternChar = pattern.At(patternIndex);
                            while (patternChar == '*')
                            {
                                patternIndex++;
                                patternChar = pattern.At(patternIndex);
                            }

                            if (!flags.HasFlag(MatchFlags.PathName))
                            {
                                matchSlash = true;
                            }
//                            const uchar *prev_p = p - 2;
//                            while (*++p == '*') {}
//                            if (!(flags & WM_PATHNAME))
//                                /* without WM_PATHNAME, '*' == '**' */
//                                match_slash = 1;
//                            else if ((prev_p < pattern || *prev_p == '/') &&
//                                     (*p == '\0' || *p == '/' ||
//                                      (p[0] == '\\' && p[1] == '/'))) {
//                                /*
//                                 * Assuming we already match 'foo/' and are at
//                                 * <star star slash>, just assume it matches
//                                 * nothing and go ahead match the rest of the
//                                 * pattern with the remaining string. This
//                                 * helps make foo/<*><*>/bar (<> because
//                                 * otherwise it breaks C comment syntax) match
//                                 * both foo/bar and foo/a/bar.
//                                 */
//                                if (p[0] == '/' &&
//                                    dowild(p + 1, text, flags) == WM_MATCH)
//                                    return WM_MATCH;
//                                match_slash = 1;
//                            } else
//                                return WM_ABORT_MALFORMED;
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

                        if (patternChar == '^')
                            patternChar = '!';
                        
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
                            if (patternChar == '\\')
                            {
                                throw new NotImplementedException();
                            }
                            else if (patternChar == '-' 
                                     && patternCharPrevious != null
                                     && pattern.At(patternIndex+1) != null
                                     && pattern.At(patternIndex+1) != ']')
                            {
                                patternIndex++;
                                patternChar = pattern.At(patternIndex);
                                // ﻿p_ch == '-' && prev_ch && p[1] && p[1] != ']'
                                if (textChar <= patternChar && textChar >= patternCharPrevious)
                                {
                                    matched = 1;
                                }
                                else if (flags.HasFlag(MatchFlags.CaseFold) && char.IsLower(textChar.Value))
                                {
                                    var textCharUpper = char.ToUpper(textChar.Value);
                                    if (textCharUpper <= patternChar && textCharUpper >= patternCharPrevious)
                                        matched = 1;
                                }
                                patternChar = null;
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
        
        public static bool Match(string pattern, string text, MatchFlags matchFlags)
        {
            var result = DoWild(pattern, text, matchFlags) == 0 ? 0 : 1;
            return result == 0;
        }
    }
}