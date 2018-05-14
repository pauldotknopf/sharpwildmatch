using System;

namespace SharpWildmatch
{
    public class Wildmatch
    {
        private static MatchResult DoWild(string pattern, string text, MatchFlags flags)
        {
            var patternIndex = 0;
            var textIndex = 0;
            for (; patternIndex < pattern.Length; patternIndex++, textIndex++)
            {
                bool matchSlash = false;
                MatchResult matched;
                int negated = 0;
                var textChar = text.At(textIndex);
                var patternChar = pattern.At(patternIndex);
                char? patternCharPrevious = null;
                
                if (textChar == null && patternChar != '*')
                    return MatchResult.AbortAll;

                switch (patternChar)
                {
                    case '\\':
                        patternIndex++;
                        patternChar = pattern.At(patternIndex);
                        // fallthrough
                        goto default;
                    default:
                        if (textChar != patternChar)
                            return MatchResult.NoMatch;
                        continue;
                    case '?':
                        if (flags.HasFlag(MatchFlags.CaseFold) && textChar == '/')
                            return MatchResult.NoMatch;
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
                                    return MatchResult.NoMatch;
                            }
                            return (int)MatchResult.Match;
                        } else if (!matchSlash && patternChar == '/') {
                            // TODO:
                            throw new NotImplementedException();
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
                                    return MatchResult.NoMatch;
                            }
                            
                            if ((matched = DoWild(pattern.Substring(patternIndex), text.Substring(textIndex), flags)) != MatchResult.NoMatch) {
                                if (!matchSlash || matched != MatchResult.AbortToStartStart)
                                    return matched;
                            } else if (!matchSlash && textChar == '/')
                                return MatchResult.AbortToStartStart;

                            textIndex++;
                            textChar = text.At(textIndex);
                        }
                        
                        return MatchResult.AbortAll;
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
                                    matched = MatchResult.NoMatch;
                                }
                                else if (flags.HasFlag(MatchFlags.CaseFold) && char.IsLower(textChar.Value))
                                {
                                    var textCharUpper = char.ToUpper(textChar.Value);
                                    if (textCharUpper <= patternChar && textCharUpper >= patternCharPrevious)
                                        matched = MatchResult.NoMatch;
                                }
                                patternChar = null;
                            }else if (false)
                            {
                                
                            }else if (patternChar == textChar)
                            {
                                matched = MatchResult.NoMatch;
                            }
                        } while (Next());
                        
                        if ((int)matched == negated ||
                            (flags.HasFlag(MatchFlags.PathName)) && textChar == '/')
                            return MatchResult.NoMatch;
                        continue;
                }
            }

            return textIndex < text.Length ? MatchResult.NoMatch : MatchResult.Match;
        }
        
        public static MatchResult Match(string pattern, string text, MatchFlags matchFlags)
        {
            var result =  DoWild(pattern, text, matchFlags);
            if(result < 0) throw new Exception($"Unexpected error: {result}");
            return (MatchResult)result;
        }
    }
}