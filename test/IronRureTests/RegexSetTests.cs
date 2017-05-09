using System;
using Xunit;

using IronRure;

namespace IronRureTets
{
    public class RegexSetTests
    {
        [Fact]
        public void RegexSet_CreateWithSingleRegex_Succeeds()
        {
            var set = new RegexSet(".+");
        }

        [Fact]
        public void RegexSet_CreateWithMultiplePatterns_Succeeds()
        {
            var set = new RegexSet("hel+o", "world", "0x[a-zA-Z0-9]{4,8}");
        }

        [Fact]
        public void RegexSet_AsIDisposable_ImplementsInterface()
        {
            var set = new RegexSet("");
            var dispo = set as IDisposable;

            Assert.NotNull(dispo);
        }

        [Fact]
        public void RegexSet_IsMatch_ReturnsTrueIfAnyRegexMatches()
        {
            using (var regs = new RegexSet("fo+", "[0-9]+"))
            {
                Assert.True(regs.IsMatch("fooooo"));
                Assert.True(regs.IsMatch("bar 1"));
                Assert.False(regs.IsMatch("no match here"));
            }
        }

        [Fact]
        public void RegexSet_Matches_ExposesMatchingPatterns()
        {
            using (var regs = new RegexSet("hello", "regex", "world"))
            {
                var match = regs.Matches("hello world");
                Assert.True(match.Matched);
                Assert.True(match.Matches[0]);
                Assert.False(match.Matches[1]);
                Assert.True(match.Matches[0]);
            }
        }
    }
}
