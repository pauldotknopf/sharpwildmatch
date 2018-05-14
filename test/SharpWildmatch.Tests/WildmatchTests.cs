using System;
using System.IO;
using FluentAssert;
using Xunit;

namespace SharpWildmatch.Tests
{
    public class WildmatchTests
    {
        [Fact]
        public void Basic_wildmatch()
        {
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
            Test(true, true, true, true, "f\\oo", "f\\\\oo");
            Test(true, true, true, true, "ball", "*[al]?");
            Test(false, false, false, false, "ten", "[ten]");
            // TODO
            //Test(false, false, true, true, "ten", "**[!te]");
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
        }

        private void Test(bool wildmatch, bool wildmatchInsenstive, bool pathmatch, bool pathmatchInsensitive, string input, string pattern)
        {
            Wildmatch.Match(pattern, input, MatchFlags.None)
                .ShouldBeEqualTo(wildmatch, $"Rule (wildmatch={wildmatch}) for input: '{input}' pattern: '{pattern}' failed.");

            Wildmatch.Match(pattern, input, MatchFlags.CaseFold)
                .ShouldBeEqualTo(wildmatchInsenstive, $"Rule (wildmatchi={wildmatchInsenstive}) for input: '{input}' pattern: '{pattern}' failed.");

            Wildmatch.Match(pattern, input, MatchFlags.PathName)
                .ShouldBeEqualTo(pathmatch, $"Rule (pathmatch={pathmatch}) for input: '{input}' pattern: '{pattern}' failed.");

            Wildmatch.Match(pattern, input, MatchFlags.PathName | MatchFlags.CaseFold)
                .ShouldBeEqualTo(pathmatchInsensitive, $"Rule (pathmatchi={pathmatchInsensitive}) for input: '{input}' pattern: '{pattern}' failed.");
        }

        private void TestGitListFiles(bool wildmatch, string input, string pattern)
        {
            using (var directory = new WorkingDirectorySession())
            {
                SimpleExec.Command.Run("git", "init", directory.Directory);

                if (!string.IsNullOrEmpty(input))
                {
                    if (!string.IsNullOrEmpty(Path.GetDirectoryName(input)))
                    {
                        Directory.CreateDirectory(Path.Combine(directory.Directory, Path.GetDirectoryName(input)));
                    }

                    File.WriteAllText(Path.Combine(directory.Directory, input), "test");
                }

                var output = SimpleExec.Command.Read("git", "--glob-pathspecs ls-files --others -z", directory.Directory);

                if (!wildmatch)
                {
                    output.ShouldNotBeNullOrEmpty();
                }
                else
                {
                    output.StartsWith(input).ShouldBeTrue();
                }
            }
        }
    }
}