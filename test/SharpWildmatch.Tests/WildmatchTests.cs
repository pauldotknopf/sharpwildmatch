using System;
using Xunit;

namespace SharpWildmatch.Tests
{
    public class WildmatchTests
    {
        [Fact]
        public void Can_glob()
        {
﻿           Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"foo", @"foo");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, @"foo", @"bar");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"", @"");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"foo", @"???");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, @"foo", @"??");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"foo", @"*");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"foo", @"f*");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, @"foo", @"*f");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"foo", @"*foo*");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"foobar", @"*ob*a*r*");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"aaaaaaabababab", @"*ab");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"foo*", @"foo\*");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, @"foobar", @"foo\*bar");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"f\oo", @"f\\oo");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"ball", @"*[al]?");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, @"ten", @"[ten]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.AbortMalformed, MatchResult.AbortMalformed, @"ten", @"**[!te]");
            Test(MatchResult.AbortAll, MatchResult.AbortAll, MatchResult.AbortMalformed, MatchResult.AbortMalformed, @"ten", @"**[!ten]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"ten", @"t[a-g]n");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, @"ten", @"t[!a-g]n");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"ton", @"t[!a-g]n");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"ton", @"t[^a-g]n");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"a]b", @"a[]]b");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"a-b", @"a[]-]b");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"a]b", @"a[]-]b");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, @"aab", @"a[]-]b");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"aab", @"a[]a-]b");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"]", @"]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.NoMatch, MatchResult.NoMatch, @"foo/baz/bar", @"foo*bar");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.AbortMalformed, MatchResult.AbortMalformed, @"foo/baz/bar", @"foo**bar");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.AbortMalformed, MatchResult.AbortMalformed, @"foobazbar", @"foo**bar");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"foo/baz/bar", @"foo/**/bar");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.Match, MatchResult.Match, @"foo/baz/bar", @"foo/**/**/bar");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"foo/b/a/z/bar", @"foo/**/bar");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"foo/b/a/z/bar", @"foo/**/**/bar");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.Match, MatchResult.Match, @"foo/bar", @"foo/**/bar");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.Match, MatchResult.Match, @"foo/bar", @"foo/**/**/bar");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.NoMatch, MatchResult.NoMatch, @"foo/bar", @"foo?bar");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.NoMatch, MatchResult.NoMatch, @"foo/bar", @"foo[/]bar");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.NoMatch, MatchResult.NoMatch, @"foo/bar", @"foo[^a-z]bar");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.NoMatch, MatchResult.NoMatch, @"foo/bar", @"f[^eiu][^eiu][^eiu][^eiu][^eiu]r");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"foo-bar", @"f[^eiu][^eiu][^eiu][^eiu][^eiu]r");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.Match, MatchResult.Match, @"foo", @"**/foo");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"XXX/foo", @"**/foo");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"bar/baz/foo", @"**/foo");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.NoMatch, MatchResult.NoMatch, @"bar/baz/foo", @"*/foo");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.NoMatch, MatchResult.NoMatch, @"foo/bar/baz", @"**/bar*");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"deep/foo/bar/baz", @"**/bar/*");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.AbortAll, MatchResult.AbortAll, @"deep/foo/bar/baz/", @"**/bar/*");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"deep/foo/bar/baz/", @"**/bar/**");
            Test(MatchResult.AbortAll, MatchResult.AbortAll, MatchResult.AbortAll, MatchResult.AbortAll, @"deep/foo/bar", @"**/bar/*");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"deep/foo/bar/", @"**/bar/**");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.AbortMalformed, MatchResult.AbortMalformed, @"foo/bar/baz", @"**/bar**");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"foo/bar/baz/x", @"*/bar/**");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.NoMatch, MatchResult.NoMatch, @"deep/foo/bar/baz/x", @"*/bar/**");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"deep/foo/bar/baz/x", @"**/bar/*/*");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, @"acrt", @"a[c-c]st");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"acrt", @"a[c-c]rt");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, @"]", @"[!]-]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"a", @"[!]-]");
            Test(MatchResult.AbortAll, MatchResult.AbortAll, MatchResult.AbortAll, MatchResult.AbortAll, @"", @"\");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, @"\", @"\");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, @"XXX/\", @"*/\");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"XXX/\", @"*/\\");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"foo", @"foo");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"@foo", @"@foo");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, @"foo", @"@foo");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"[ab]", @"\[ab]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"[ab]", @"[[]ab]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"[ab]", @"[[:]ab]");
            Test(MatchResult.AbortAll, MatchResult.AbortAll, MatchResult.AbortAll, MatchResult.AbortAll, @"[ab]", @"[[::]ab]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"[ab]", @"[[:digit]ab]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"[ab]", @"[\[:]ab]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"?a?b", @"\??\?b");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"abc", @"\a\b\c");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, @"foo", @"");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"foo/bar/baz/to", @"**/t[o]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"a1B", @"[[:alpha:]][[:digit:]][[:upper:]]");
            Test(MatchResult.NoMatch, MatchResult.Match, MatchResult.NoMatch, MatchResult.Match, @"a", @"[[:digit:][:upper:][:space:]]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"A", @"[[:digit:][:upper:][:space:]]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"1", @"[[:digit:][:upper:][:space:]]");
            Test(MatchResult.AbortAll, MatchResult.AbortAll, MatchResult.AbortAll, MatchResult.AbortAll, @"1", @"[[:digit:][:upper:][:spaci:]]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @" ", @"[[:digit:][:upper:][:space:]]");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, @".", @"[[:digit:][:upper:][:space:]]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @".", @"[[:digit:][:punct:][:space:]]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"5", @"[[:xdigit:]]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"f", @"[[:xdigit:]]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"D", @"[[:xdigit:]]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"_", @"[[:alnum:][:alpha:][:blank:][:cntrl:][:digit:][:graph:][:lower:][:print:][:punct:][:space:][:upper:][:xdigit:]]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @".", @"[^[:alnum:][:alpha:][:blank:][:cntrl:][:digit:][:lower:][:space:][:upper:][:xdigit:]]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"5", @"[a-c[:digit:]x-z]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"b", @"[a-c[:digit:]x-z]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"y", @"[a-c[:digit:]x-z]");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, @"q", @"[a-c[:digit:]x-z]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"]", @"[\\-^]");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, @"[", @"[\\-^]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"-", @"[\-_]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"]", @"[\]]");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, @"\]", @"[\]]");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, @"\", @"[\]]");
            Test(MatchResult.AbortAll, MatchResult.AbortAll, MatchResult.AbortAll, MatchResult.AbortAll, @"ab", @"a[]b");
            Test(MatchResult.AbortAll, MatchResult.AbortAll, MatchResult.AbortAll, MatchResult.AbortAll, @"a[]b", @"a[]b");
            Test(MatchResult.AbortAll, MatchResult.AbortAll, MatchResult.AbortAll, MatchResult.AbortAll, @"ab[", @"ab[");
            Test(MatchResult.AbortAll, MatchResult.AbortAll, MatchResult.AbortAll, MatchResult.AbortAll, @"ab", @"[!");
            Test(MatchResult.AbortAll, MatchResult.AbortAll, MatchResult.AbortAll, MatchResult.AbortAll, @"ab", @"[-");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"-", @"[-]");
            Test(MatchResult.AbortAll, MatchResult.AbortAll, MatchResult.AbortAll, MatchResult.AbortAll, @"-", @"[a-");
            Test(MatchResult.AbortAll, MatchResult.AbortAll, MatchResult.AbortAll, MatchResult.AbortAll, @"-", @"[!a-");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"-", @"[--A]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"5", @"[--A]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @" ", @"[ --]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"$", @"[ --]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"-", @"[ --]");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, @"0", @"[ --]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"-", @"[---]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"-", @"[------]");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, @"j", @"[a-e-n]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"-", @"[a-e-n]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"a", @"[!------]");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, @"[", @"[]-a]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"^", @"[]-a]");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, @"^", @"[!]-a]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"[", @"[!]-a]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"^", @"[a^bc]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"-b]", @"[a-]b]");
            Test(MatchResult.AbortAll, MatchResult.AbortAll, MatchResult.AbortAll, MatchResult.AbortAll, @"\", @"[\]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"\", @"[\\]");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, @"\", @"[!\\]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"G", @"[A-\\]");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, @"aaabbb", @"b*a");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, @"aabcaa", @"*ba*");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @",", @"[,]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @",", @"[\\,]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"\", @"[\\,]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"-", @"[,-.]");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, @"+", @"[,-.]");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, @"-.]", @"[,-.]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"2", @"[\1-\3]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"3", @"[\1-\3]");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, @"4", @"[\1-\3]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"\", @"[[-\]]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"[", @"[[-\]]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"]", @"[[-\]]");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, @"-", @"[[-\]]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"-adobe-courier-bold-o-normal--12-120-75-75-m-70-iso8859-1", @"-*-*-*-*-*-*-12-*-*-*-m-*-*-*");
            Test(MatchResult.AbortAll, MatchResult.AbortAll, MatchResult.AbortAll, MatchResult.AbortAll, @"-adobe-courier-bold-o-normal--12-120-75-75-X-70-iso8859-1", @"-*-*-*-*-*-*-12-*-*-*-m-*-*-*");
            Test(MatchResult.AbortAll, MatchResult.AbortAll, MatchResult.NoMatch, MatchResult.NoMatch, @"-adobe-courier-bold-o-normal--12-120-75-75-/-70-iso8859-1", @"-*-*-*-*-*-*-12-*-*-*-m-*-*-*");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"XXX/adobe/courier/bold/o/normal//12/120/75/75/m/70/iso8859/1", @"XXX/*/*/*/*/*/*/12/*/*/*/m/*/*/*");
            Test(MatchResult.AbortAll, MatchResult.AbortAll, MatchResult.NoMatch, MatchResult.NoMatch, @"XXX/adobe/courier/bold/o/normal//12/120/75/75/X/70/iso8859/1", @"XXX/*/*/*/*/*/*/12/*/*/*/m/*/*/*");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"abcd/abcdefg/abcdefghijk/abcdefghijklmnop.txt", @"**/*a*b*g*n*t");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, @"abcd/abcdefg/abcdefghijk/abcdefghijklmnop.txtz", @"**/*a*b*g*n*t");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, @"foo", @"*/*/*");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, @"foo/bar", @"*/*/*");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"foo/bba/arr", @"*/*/*");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.NoMatch, MatchResult.NoMatch, @"foo/bb/aa/rr", @"*/*/*");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"foo/bb/aa/rr", @"**/**/**");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"abcXdefXghi", @"*X*i");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.NoMatch, MatchResult.NoMatch, @"ab/cXd/efXg/hi", @"*X*i");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"ab/cXd/efXg/hi", @"*/*X*/*/*i");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"ab/cXd/efXg/hi", @"**/*X*/**/*i");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, @"foo", @"fo");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"foo/bar", @"foo/bar");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"foo/bar", @"foo/*");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.NoMatch, MatchResult.NoMatch, @"foo/bba/arr", @"foo/*");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"foo/bba/arr", @"foo/**");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.NoMatch, MatchResult.NoMatch, @"foo/bba/arr", @"foo*");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.AbortMalformed, MatchResult.AbortMalformed, @"foo/bba/arr", @"foo**");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.NoMatch, MatchResult.NoMatch, @"foo/bba/arr", @"foo/*arr");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.AbortMalformed, MatchResult.AbortMalformed, @"foo/bba/arr", @"foo/**arr");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.NoMatch, @"foo/bba/arr", @"foo/*z");
            Test(MatchResult.NoMatch, MatchResult.NoMatch, MatchResult.AbortMalformed, MatchResult.AbortMalformed, @"foo/bba/arr", @"foo/**z");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.NoMatch, MatchResult.NoMatch, @"foo/bar", @"foo?bar");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.NoMatch, MatchResult.NoMatch, @"foo/bar", @"foo[/]bar");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.NoMatch, MatchResult.NoMatch, @"foo/bar", @"foo[^a-z]bar");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.NoMatch, MatchResult.NoMatch, @"ab/cXd/efXg/hi", @"*Xg*i");
            Test(MatchResult.NoMatch, MatchResult.Match, MatchResult.NoMatch, MatchResult.Match, @"a", @"[A-Z]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"A", @"[A-Z]");
            Test(MatchResult.NoMatch, MatchResult.Match, MatchResult.NoMatch, MatchResult.Match, @"A", @"[a-z]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"a", @"[a-z]");
            Test(MatchResult.NoMatch, MatchResult.Match, MatchResult.NoMatch, MatchResult.Match, @"a", @"[[:upper:]]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"A", @"[[:upper:]]");
            Test(MatchResult.NoMatch, MatchResult.Match, MatchResult.NoMatch, MatchResult.Match, @"A", @"[[:lower:]]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"a", @"[[:lower:]]");
            Test(MatchResult.NoMatch, MatchResult.Match, MatchResult.NoMatch, MatchResult.Match, @"A", @"[B-Za]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"a", @"[B-Za]");
            Test(MatchResult.NoMatch, MatchResult.Match, MatchResult.NoMatch, MatchResult.Match, @"A", @"[B-a]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"a", @"[B-a]");
            Test(MatchResult.NoMatch, MatchResult.Match, MatchResult.NoMatch, MatchResult.Match, @"z", @"[Z-y]");
            Test(MatchResult.Match, MatchResult.Match, MatchResult.Match, MatchResult.Match, @"Z", @"[Z-y]");
        }
        
        private void Test(
            // ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local
            MatchResult wildmatch,
            MatchResult wildmatchInsenstive,
            MatchResult pathmatch,
            MatchResult pathmatchInsensitive,
            // ReSharper restore ParameterOnlyUsedForPreconditionCheck.Local
            string input,
            string pattern)
        {
            var wildmatchResult = Wildmatch.Match(pattern, input, MatchFlags.None);
            var wildmatchInsenstiveResult = Wildmatch.Match(pattern, input, MatchFlags.CaseFold);
            var pathmatchResult = Wildmatch.Match(pattern, input, MatchFlags.PathName);
            var pathmatchInsensitiveResult = Wildmatch.Match(pattern, input, MatchFlags.PathName | MatchFlags.CaseFold);

            try
            {
                Assert.Equal($"{wildmatch}:{wildmatchInsenstive}:{pathmatch}:{pathmatchInsensitive}",
                    $"{wildmatchResult}:{wildmatchInsenstiveResult}:{pathmatchResult}:{pathmatchInsensitiveResult}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Input: {input} Pattern: {pattern} Message: {ex.Message}");
            }
        }
    }
}