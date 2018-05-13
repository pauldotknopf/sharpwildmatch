using FluentAssert;
using Xunit;

namespace SharpWildmatch.Tests
{
    public class WildmatchTests
    {
        [Fact]
        public void Base_wildmatch()
        {
            //Test(false, false, false, f, "foo", "??");
            //return;
            
            //# Basic wildTest(features
            Test(true, true, true, true, "foo", "foo");
            Test(false, false, false, false, "foo", "bar");
            Test(true, true, true, true, "", "");
            Test(true, true, true, true, "foo", "???");
            Test(false, false, false, false, "foo", "??");
            Test(true, true, true, true, "foo", "*");
            Test(true, true, true, true, "foo", "f*");
            Test(false, false, false, false, "foo", "*f");
            Test(true, true, true, true, "foo", "*foo*");
            Test(true, true, true, true, "foobar", "*ob*a*r*");
            Test(true, true, true, true, "aaaaaaabababab", "*ab");
            Test(true, true, true, true, "foo*", "foo\\*");
            Test(false, false, false, false, "foobar", "foo\\*bar");
            Test(true, true, true, true, "f\\oo", "f\\oo");
            Test(true, true, true, true, "ball", "*[al]?");
            Test(false, false, false, false, "ten", "[ten]");
            Test(false, false, true, true, "ten", "**[!te]");
            Test(false, false, false, false, "ten", "**[!ten]");
            Test(true, true, true, true, "ten", "t[a-g]n");
            Test(false, false, false, false, "ten", "t[!a-g]n");
            Test(true, true, true, true, "ton", "t[!a-g]n");
            Test(true, true, true, true, "ton", "t[^a-g]n");
            Test(true, true, true, true, "a]b", "a[]]b");
            Test(true, true, true, true, "a-b", "a[]-]b");
            Test(true, true, true, true, "a]b", "a[]-]b");
            Test(false, false, false, false, "aab", "a[]-]b");
            Test(true, true, true, true, "aab", "a[]a-]b");
            Test(true, true, true, true, "]", "]");
            
//            # Extended slash-matching features
//            Test(false, false, true, true, "foo/baz/bar" "foo*bar"
//            Test(false, false, true, true, "foo/baz/bar" "foo**bar"
//            Test(false, false, true, true, "foobazbar" "foo**bar"
//            Test(true, true, true, true, "foo/baz/bar" "foo/**/bar"
//            Test(true, true, false, false, "foo/baz/bar" "foo/**/**/bar"
//            Test(true, true, true, true, "foo/b/a/z/bar" "foo/**/bar"
//            Test(true, true, true, true, "foo/b/a/z/bar" "foo/**/**/bar"
//            Test(true, true, false, false, "foo/bar" "foo/**/bar"
//            Test(true, true, false, false, "foo/bar" "foo/**/**/bar"
//            Test(false, false, true, true, "foo/bar" "foo?bar"
//            Test(false, false, true, true, "foo/bar" "foo[/]bar"
//            Test(false, false, true, true, "foo/bar" "foo[^a-z]bar"
//            Test(false, false, true, true, "foo/bar" "f[^eiu][^eiu][^eiu][^eiu][^eiu]r"
//            Test(true, true, true, true, "foo-bar" "f[^eiu][^eiu][^eiu][^eiu][^eiu]r"
//            Test(true, true, false, false, "foo" "**/foo"
//            Test(true, true, true, true, "XXX/foo" "**/foo"
//            Test(true, true, true, true, "bar/baz/foo" "**/foo"
//            Test(false, false, true, true, "bar/baz/foo" "*/foo"
//            Test(false, false, true, true, "foo/bar/baz" "**/bar*"
//            Test(true, true, true, true, "deep/foo/bar/baz" "**/bar/*"
//            Test(false, false, true, true, "deep/foo/bar/baz/" "**/bar/*"
//            Test(true, true, true, true, "deep/foo/bar/baz/" "**/bar/**"
//            Test(false, false, false, false, "deep/foo/bar" "**/bar/*"
//            Test(true, true, true, true, "deep/foo/bar/" "**/bar/**"
//            Test(false, false, true, true, "foo/bar/baz" "**/bar**"
//            Test(true, true, true, true, "foo/bar/baz/x" "*/bar/**"
//            Test(false, false, true, true, "deep/foo/bar/baz/x" "*/bar/**"
//            Test(true, true, true, true, "deep/foo/bar/baz/x" "**/bar/*/*"
//            
//            # Various additional tests
//            Test(false, false, false, false, "acrt" "a[c-c]st"
//            Test(true, true, true, true, "acrt" "a[c-c]rt"
//            Test(false, false, false, false, "]" "[!]-]"
//            Test(true, true, true, true, "a" "[!]-]"
//            Test(false, false, false, false, "" "\"
//            Test(false, false, false, false, \
//                  true, true, true, true, "\" "\"
//            Test(false, false, false, false, "XXX/\" "*/\"
//            Test(true, true, true, true, "XXX/\" "*/\\"
//            Test(true, true, true, true, "foo" "foo"
//            Test(true, true, true, true, "@foo" "@foo"
//            Test(false, false, false, false, "foo" "@foo"
//            Test(true, true, true, true, "[ab]" "\[ab]"
//            Test(true, true, true, true, "[ab]" "[[]ab]"
//            Test(true, true, true, true, "[ab]" "[[:]ab]"
//            Test(false, false, false, false, "[ab]" "[[::]ab]"
//            Test(true, true, true, true, "[ab]" "[[:digit]ab]"
//            Test(true, true, true, true, "[ab]" "[\[:]ab]"
//            Test(true, true, true, true, "?a?b" "\??\?b"
//            Test(true, true, true, true, "abc" "\a\b\c"
//            Test(false, false, false, false, \
//                  E E E E "foo" ""
//            Test(true, true, true, true, "foo/bar/baz/to" "**/t[o]"
//            
//            # Character class tests
//            Test(true, true, true, true, "a1B" "[[:alpha:]][[:digit:]][[:upper:]]"
//            Test(false, true, false, true, "a" "[[:digit:][:upper:][:space:]]"
//            Test(true, true, true, true, "A" "[[:digit:][:upper:][:space:]]"
//            Test(true, true, true, true, "1" "[[:digit:][:upper:][:space:]]"
//            Test(false, false, false, false, "1" "[[:digit:][:upper:][:spaci:]]"
//            Test(true, true, true, true, " " "[[:digit:][:upper:][:space:]]"
//            Test(false, false, false, false, "." "[[:digit:][:upper:][:space:]]"
//            Test(true, true, true, true, "." "[[:digit:][:punct:][:space:]]"
//            Test(true, true, true, true, "5" "[[:xdigit:]]"
//            Test(true, true, true, true, "f" "[[:xdigit:]]"
//            Test(true, true, true, true, "D" "[[:xdigit:]]"
//            Test(true, true, true, true, "_" "[[:alnum:][:alpha:][:blank:][:cntrl:][:digit:][:graph:][:lower:][:print:][:punct:][:space:][:upper:][:xdigit:]]"
//            Test(true, true, true, true, "." "[^[:alnum:][:alpha:][:blank:][:cntrl:][:digit:][:lower:][:space:][:upper:][:xdigit:]]"
//            Test(true, true, true, true, "5" "[a-c[:digit:]x-z]"
//            Test(true, true, true, true, "b" "[a-c[:digit:]x-z]"
//            Test(true, true, true, true, "y" "[a-c[:digit:]x-z]"
//            Test(false, false, false, false, "q" "[a-c[:digit:]x-z]"
//            
//            # Additional tests, including some malformed wildTest(patterns
//            Test(true, true, true, true, "]" "[\\-^]"
//            Test(false, false, false, false, "[" "[\\-^]"
//            Test(true, true, true, true, "-" "[\-_]"
//            Test(true, true, true, true, "]" "[\]]"
//            Test(false, false, false, false, "\]" "[\]]"
//            Test(false, false, false, false, "\" "[\]]"
//            Test(false, false, false, false, "ab" "a[]b"
//            Test(false, false, false, false, \
//                  true, true, true, true, "a[]b" "a[]b"
//            Test(false, false, false, false, \
//                  true, true, true, true, "ab[" "ab["
//            Test(false, false, false, false, "ab" "[!"
//            Test(false, false, false, false, "ab" "[-"
//            Test(true, true, true, true, "-" "[-]"
//            Test(false, false, false, false, "-" "[a-"
//            Test(false, false, false, false, "-" "[!a-"
//            Test(true, true, true, true, "-" "[--A]"
//            Test(true, true, true, true, "5" "[--A]"
//            Test(true, true, true, true, " " "[ --]"
//            Test(true, true, true, true, "$" "[ --]"
//            Test(true, true, true, true, "-" "[ --]"
//            Test(false, false, false, false, "0" "[ --]"
//            Test(true, true, true, true, "-" "[---]"
//            Test(true, true, true, true, "-" "[------]"
//            Test(false, false, false, false, "j" "[a-e-n]"
//            Test(true, true, true, true, "-" "[a-e-n]"
//            Test(true, true, true, true, "a" "[!------]"
//            Test(false, false, false, false, "[" "[]-a]"
//            Test(true, true, true, true, "^" "[]-a]"
//            Test(false, false, false, false, "^" "[!]-a]"
//            Test(true, true, true, true, "[" "[!]-a]"
//            Test(true, true, true, true, "^" "[a^bc]"
//            Test(true, true, true, true, "-b]" "[a-]b]"
//            Test(false, false, false, false, "\" "[\]"
//            Test(true, true, true, true, "\" "[\\]"
//            Test(false, false, false, false, "\" "[!\\]"
//            Test(true, true, true, true, "G" "[A-\\]"
//            Test(false, false, false, false, "aaabbb" "b*a"
//            Test(false, false, false, false, "aabcaa" "*ba*"
//            Test(true, true, true, true, "," "[,]"
//            Test(true, true, true, true, "," "[\\,]"
//            Test(true, true, true, true, "\" "[\\,]"
//            Test(true, true, true, true, "-" "[,-.]"
//            Test(false, false, false, false, "+" "[,-.]"
//            Test(false, false, false, false, "-.]" "[,-.]"
//            Test(true, true, true, true, "2" "[\1-\3]"
//            Test(true, true, true, true, "3" "[\1-\3]"
//            Test(false, false, false, false, "4" "[\1-\3]"
//            Test(true, true, true, true, "\" "[[-\]]"
//            Test(true, true, true, true, "[" "[[-\]]"
//            Test(true, true, true, true, "]" "[[-\]]"
//            Test(false, false, false, false, "-" "[[-\]]"
//            
//            # Test recursion
//            Test(true, true, true, true, "-adobe-courier-bold-o-normal--12-120-75-75-m-70-iso8859-1" "-*-*-*-*-*-*-12-*-*-*-m-*-*-*"
//            Test(false, false, false, false, "-adobe-courier-bold-o-normal--12-120-75-75-X-70-iso8859-1" "-*-*-*-*-*-*-12-*-*-*-m-*-*-*"
//            Test(false, false, false, false, "-adobe-courier-bold-o-normal--12-120-75-75-/-70-iso8859-1" "-*-*-*-*-*-*-12-*-*-*-m-*-*-*"
//            Test(true, true, true, true, "XXX/adobe/courier/bold/o/normal//12/120/75/75/m/70/iso8859/1" "XXX/*/*/*/*/*/*/12/*/*/*/m/*/*/*"
//            Test(false, false, false, false, "XXX/adobe/courier/bold/o/normal//12/120/75/75/X/70/iso8859/1" "XXX/*/*/*/*/*/*/12/*/*/*/m/*/*/*"
//            Test(true, true, true, true, "abcd/abcdefg/abcdefghijk/abcdefghijklmnop.txt" "**/*a*b*g*n*t"
//            Test(false, false, false, false, "abcd/abcdefg/abcdefghijk/abcdefghijklmnop.txtz" "**/*a*b*g*n*t"
//            Test(false, false, false, false, foo "*/*/*"
//            Test(false, false, false, false, foo/bar "*/*/*"
//            Test(true, true, true, true, foo/bba/arr "*/*/*"
//            Test(false, false, true, true, foo/bb/aa/rr "*/*/*"
//            Test(true, true, true, true, foo/bb/aa/rr "**/**/**"
//            Test(true, true, true, true, abcXdefXghi "*X*i"
//            Test(false, false, true, true, ab/cXd/efXg/hi "*X*i"
//            Test(true, true, true, true, ab/cXd/efXg/hi "*/*X*/*/*i"
//            Test(true, true, true, true, ab/cXd/efXg/hi "**/*X*/**/*i"
//            
//            # Extra pathTest(tests
//            Test(false, false, false, false, foo fo
//            Test(true, true, true, true, foo/bar foo/bar
//            Test(true, true, true, true, foo/bar "foo/*"
//            Test(false, false, true, true, foo/bba/arr "foo/*"
//            Test(true, true, true, true, foo/bba/arr "foo/**"
//            Test(false, false, true, true, foo/bba/arr "foo*"
//            Test(false, false, true, true, \
//                  true, true, true, true, foo/bba/arr "foo**"
//            Test(false, false, true, true, foo/bba/arr "foo/*arr"
//            Test(false, false, true, true, foo/bba/arr "foo/**arr"
//            Test(false, false, false, false, foo/bba/arr "foo/*z"
//            Test(false, false, false, false, foo/bba/arr "foo/**z"
//            Test(false, false, true, true, foo/bar "foo?bar"
//            Test(false, false, true, true, foo/bar "foo[/]bar"
//            Test(false, false, true, true, foo/bar "foo[^a-z]bar"
//            Test(false, false, true, true, ab/cXd/efXg/hi "*Xg*i"
//            
//            # Extra case-sensitivity tests
//            Test(false, true, false, true, "a" "[A-Z]"
//            Test(true, true, true, true, "A" "[A-Z]"
//            Test(false, true, false, true, "A" "[a-z]"
//            Test(true, true, true, true, "a" "[a-z]"
//            Test(false, true, false, true, "a" "[[:upper:]]"
//            Test(true, true, true, true, "A" "[[:upper:]]"
//            Test(false, true, false, true, "A" "[[:lower:]]"
//            Test(true, true, true, true, "a" "[[:lower:]]"
//            Test(false, true, false, true, "A" "[B-Za]"
//            Test(true, true, true, true, "a" "[B-Za]"
//            Test(false, true, false, true, "A" "[B-a]"
//            Test(true, true, true, true, "a" "[B-a]"
//            Test(false, true, false, true, "z" "[Z-y]"
//            Test(true, true, true, true, "Z" "[Z-y]"

        }

        private void Test(bool glob, bool globInsenstive, bool pathmatch, bool pathmatchInsensitive, string input, string pattern)
        {
            Wildmatch.Match(pattern, input, 0).ShouldBeEqualTo(glob, $"Rule for input: '{input}' pattern: '{pattern}' failed.");
        }
    }
}