using System;
using System.Linq;
using Xunit;

namespace IronRure.DropInTests
{
    using IronRure.DropIn;

    public class RegexTests
    {
        [Fact]
        public void CreateWithSimplePattern()
        {
            var rege = new Regex("hello world");
        }

        [Fact]
        public void CreateWithPatternAndOptions()
        {
            var regex = new Regex("^hello.*world$", RegexOptions.Singleline);
        }

        [Fact]
        public void CreateWithOptionsAndTimeout()
        {
            var regex = new Regex(@"\b[at]\w+", RegexOptions.None, TimeSpan.FromMinutes(1));
        }

        [Fact]
        public void UnsupportedRegexOptions()
        {
            Assert.Throws<NotSupportedException>(() => new Regex("a", RegexOptions.RightToLeft));
            Assert.Throws<NotSupportedException>(() => new Regex("a", RegexOptions.ECMAScript));
        }

        [Fact]
        public void IsMatchWithSimpleWordPattern()
        {
            var regex = new Regex("Hello", RegexOptions.IgnoreCase);

            Assert.True(regex.IsMatch("hello world!"));
            Assert.False(regex.IsMatch("goodbye then"));
            Assert.True(regex.IsMatch("so I said hello to her", 10));
            Assert.False(regex.IsMatch("not a hello in sight", 7));
        }

        [Fact]
        public void StaticIsMatch()
        {
            Assert.False(Regex.IsMatch("hello world!", "HELLO"));
            Assert.True(Regex.IsMatch("hello world!", "HELLO", RegexOptions.IgnoreCase));
            Assert.False(Regex.IsMatch("goodbye then", "hello"));
            Assert.False(Regex.IsMatch("so I said\nhello to her", "^hello", RegexOptions.Singleline, TimeSpan.FromSeconds(10)));
            Assert.True(Regex.IsMatch("so I said\nhello to her", "^hello", RegexOptions.Multiline, TimeSpan.FromSeconds(10)));
        }

        // -----------------------------------------------------------------------
        // Match / NextMatch
        // -----------------------------------------------------------------------

        [Fact]
        public void MatchReturnsSuccessfulMatch()
        {
            var regex = new Regex(@"\d+");
            var m = regex.Match("abc 123 def");

            Assert.True(m.Success);
            Assert.Equal("123", m.Value);
            Assert.Equal(4, m.Index);
            Assert.Equal(3, m.Length);
        }

        [Fact]
        public void MatchReturnsEmptyOnNoMatch()
        {
            var regex = new Regex(@"\d+");
            var m = regex.Match("no digits here");

            Assert.False(m.Success);
        }

        [Fact]
        public void MatchWithStartAt()
        {
            var regex = new Regex(@"\d+");
            // "123 456" – skip past "123", start at index 4
            var m = regex.Match("123 456", 4);

            Assert.True(m.Success);
            Assert.Equal("456", m.Value);
            Assert.Equal(4, m.Index);
        }

        [Fact]
        public void NextMatchIterates()
        {
            var regex = new Regex(@"\d+");
            var m = regex.Match("1 22 333");

            Assert.True(m.Success); Assert.Equal("1", m.Value);
            m = m.NextMatch();
            Assert.True(m.Success); Assert.Equal("22", m.Value);
            m = m.NextMatch();
            Assert.True(m.Success); Assert.Equal("333", m.Value);
            m = m.NextMatch();
            Assert.False(m.Success);
        }

        [Fact]
        public void StaticMatch()
        {
            var m = Regex.Match("hello world", @"\w+");
            Assert.True(m.Success);
            Assert.Equal("hello", m.Value);
        }

        // -----------------------------------------------------------------------
        // Matches
        // -----------------------------------------------------------------------

        [Fact]
        public void MatchesReturnsAllMatches()
        {
            var regex = new Regex(@"\d+");
            var mc = regex.Matches("1 22 333 4444");

            Assert.Equal(4, mc.Count);
            Assert.Equal("1", mc[0].Value);
            Assert.Equal("22", mc[1].Value);
            Assert.Equal("333", mc[2].Value);
            Assert.Equal("4444", mc[3].Value);
        }

        [Fact]
        public void MatchesEmptyCollectionOnNoMatch()
        {
            var mc = new Regex(@"\d+").Matches("no digits");
            Assert.Empty(mc);
        }

        [Fact]
        public void StaticMatches()
        {
            var mc = Regex.Matches("one 1 two 2 three 3", @"\d");
            Assert.Equal(3, mc.Count);
        }

        // -----------------------------------------------------------------------
        // Groups
        // -----------------------------------------------------------------------

        [Fact]
        public void MatchGroupsIndexedByNumber()
        {
            var regex = new Regex(@"(\d{4})-(\d{2})-(\d{2})");
            var m = regex.Match("Date: 2024-03-15");

            Assert.True(m.Success);
            Assert.Equal("2024-03-15", m.Groups[0].Value);
            Assert.Equal("2024", m.Groups[1].Value);
            Assert.Equal("03", m.Groups[2].Value);
            Assert.Equal("15", m.Groups[3].Value);
        }

        [Fact]
        public void MatchGroupsIndexedByName()
        {
            var regex = new Regex(@"(?P<year>\d{4})-(?P<month>\d{2})-(?P<day>\d{2})");
            var m = regex.Match("2024-03-15");

            Assert.True(m.Success);
            Assert.Equal("2024", m.Groups["year"].Value);
            Assert.Equal("03", m.Groups["month"].Value);
            Assert.Equal("15", m.Groups["day"].Value);
        }

        [Fact]
        public void UnmatchedGroupReturnsFalse()
        {
            var regex = new Regex(@"(\d+)?");
            var m = regex.Match("no digits");
            // Group 0 matches the empty string at position 0
            Assert.False(m.Groups[1].Success);
        }

        // -----------------------------------------------------------------------
        // Replace
        // -----------------------------------------------------------------------

        [Fact]
        public void ReplaceAllMatches()
        {
            var result = new Regex(@"\d+").Replace("1 22 333", "NUM");
            Assert.Equal("NUM NUM NUM", result);
        }

        [Fact]
        public void ReplaceWithCount()
        {
            var result = new Regex(@"\d+").Replace("1 22 333", "NUM", 2);
            Assert.Equal("NUM NUM 333", result);
        }

        [Fact]
        public void ReplaceWithGroupBackreference()
        {
            var result = new Regex(@"(\w+)\s(\w+)").Replace("hello world", "$2 $1");
            Assert.Equal("world hello", result);
        }

        [Fact]
        public void ReplaceWithNamedGroupBackreference()
        {
            var result = new Regex(@"(?P<first>\w+)\s(?P<second>\w+)").Replace(
                "hello world", "${second} ${first}");
            Assert.Equal("world hello", result);
        }

        [Fact]
        public void ReplaceWithEscapedDollar()
        {
            var result = new Regex(@"\d+").Replace("cost 100", "$$");
            Assert.Equal("cost $", result);
        }

        [Fact]
        public void ReplaceWithMatchEvaluator()
        {
            var result = new Regex(@"\d+").Replace("1 22 333",
                m => (int.Parse(m.Value) * 2).ToString());
            Assert.Equal("2 44 666", result);
        }

        [Fact]
        public void StaticReplace()
        {
            var result = Regex.Replace("foo bar baz", @"\b\w", m => m.Value.ToUpper());
            Assert.Equal("Foo Bar Baz", result);
        }

        // -----------------------------------------------------------------------
        // Split
        // -----------------------------------------------------------------------

        [Fact]
        public void SplitOnDelimiter()
        {
            var parts = new Regex(@"\s+").Split("one two   three");
            Assert.Equal(new[] { "one", "two", "three" }, parts);
        }

        [Fact]
        public void SplitWithCount()
        {
            var parts = new Regex(@"\s+").Split("one two three four", 2);
            Assert.Equal(new[] { "one", "two three four" }, parts);
        }

        [Fact]
        public void StaticSplit()
        {
            var parts = Regex.Split("a1b2c3d", @"\d");
            Assert.Equal(new[] { "a", "b", "c", "d" }, parts);
        }

        // -----------------------------------------------------------------------
        // GetGroupNames / GroupNameFromNumber / GroupNumberFromName
        // -----------------------------------------------------------------------

        [Fact]
        public void GetGroupNamesIncludesZero()
        {
            var regex = new Regex(@"(?P<year>\d{4})-(\d{2})");
            var names = regex.GetGroupNames();
            Assert.Contains("0", names);
            Assert.Contains("year", names);
        }

        [Fact]
        public void GroupNameFromNumberRoundtrip()
        {
            var regex = new Regex(@"(?P<word>\w+)");
            Assert.Equal("0", regex.GroupNameFromNumber(0));
            Assert.Equal("word", regex.GroupNameFromNumber(1));
        }

        [Fact]
        public void GroupNumberFromNameRoundtrip()
        {
            var regex = new Regex(@"(?P<word>\w+)");
            Assert.Equal(0, regex.GroupNumberFromName("0"));
            Assert.Equal(1, regex.GroupNumberFromName("word"));
        }

        // -----------------------------------------------------------------------
        // ToString / Escape / Unescape
        // -----------------------------------------------------------------------

        [Fact]
        public void ToStringReturnsPattern()
        {
            var pattern = @"\d+\s\w+";
            var regex = new Regex(pattern);
            Assert.Equal(pattern, regex.ToString());
        }

        [Fact]
        public void EscapeAndUnescape()
        {
            var raw = "hello.world+foo?";
            var escaped = Regex.Escape(raw);
            Assert.Equal(raw, Regex.Unescape(escaped));
        }

        // -----------------------------------------------------------------------
        // Result
        // -----------------------------------------------------------------------

        [Fact]
        public void MatchResultExpandsReplacement()
        {
            var m = new Regex(@"(\w+)\s(\w+)").Match("hello world");
            Assert.True(m.Success);
            Assert.Equal("world hello", m.Result("$2 $1"));
        }
    }
}
