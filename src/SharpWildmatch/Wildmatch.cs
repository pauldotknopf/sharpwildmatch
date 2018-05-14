﻿using System;

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
                        if (flags.HasFlag(MatchFlags.PathName) && textChar == '/')
                            return MatchResult.NoMatch;
                        continue;
                    case '*':
                        patternIndex++;
                        patternChar = pattern.At(patternIndex);
                        if (pattern.At(patternIndex) == '*')
                        {
                            var previousIndex = patternIndex - 2;
                            var previous = pattern.At(previousIndex);
                            
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
                                // TODO: this doesn't seem right
                            }else if ((previousIndex < 0 || previous == '/') &&
                                  (patternChar == null || patternChar == '/' ||
                                      (patternChar == '\\' && pattern.At(patternIndex+1) == '/')))
                            {
                                if (patternChar == '/' &&
                                    DoWild(pattern.Substring(patternIndex + 1), text.Substring(textIndex), flags) ==
                                    MatchResult.Match)
                                    return MatchResult.Match;
                                matchSlash = true;
                            }
                            else
                            {
                                return MatchResult.AbortMalformed;
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
                                var temp = text.Substring(textIndex);
                                if (text.Substring(textIndex).Contains("/"))
                                    return MatchResult.NoMatch;
                            }
                            return (int)MatchResult.Match;
                        } else if (!matchSlash && patternChar == '/') {
                            int nextIndex = text.Substring(textIndex).IndexOf('/');
                            if (nextIndex == -1)
                            {
                                return MatchResult.NoMatch;
                            }

                            textIndex += nextIndex;
                            textChar = text.At(textIndex);
                            break;
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
            return DoWild(pattern, text, matchFlags);
        }
    }
}