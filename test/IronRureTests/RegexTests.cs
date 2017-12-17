using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

using IronRure;

namespace IronRureTests
{
    public class RegexTests
    {
        [Fact]
        public void Regex_CreateWithEmptyPattern_Succeeds()
        {
            var regex = new Regex("");
        }

        [Fact]
        public void Regex_AsIDsiposable_ImplementsInterface()
        {
            var reg = new Regex(".*");
            var dispo = reg as IDisposable;
            Assert.NotNull(dispo);
        }

        [Fact]
        public void Regex_CreateWithOptions_Succeeds()
        {
            using (var reg = new Regex(@"\w+", new Options().WithSize(512).WithDfaSize(512)))
            {
                Assert.True(reg.IsMatch("hello"));
                Assert.False(reg.IsMatch("!@£$"));
            }
        }

        [Fact]
        public void Regex_CreateWithFlags_Succeeds()
        {
            using (var reg = new Regex("", new Options(), Regex.DefaultFlags))
            {
                Assert.True(reg.IsMatch(""));
            }
        }

        [Fact]
        public void Regex_CreateWithFlagsOnly_Succeeds()
        {
            using (var reg = new Regex(@"Δ", RureFlags.Unicode | RureFlags.Casei))
            {
                Assert.True(reg.IsMatch("δ"));
            }
        }

        [Fact]
        public void Regex_CreateWithInvalidPattern_ThrowsException()
        {
            Assert.Throws<RegexCompilationException>(() => new Regex(")"));
        }

        [Fact]
        public void Regex_IsMatch_ReturnsTrueWhenPatternMatches()
        {
            var reg = new Regex("(abb)|c");

            Assert.False(reg.IsMatch("world"));
            Assert.True(reg.IsMatch("circle"));
            Assert.True(reg.IsMatch("abb circle"));
            Assert.True(reg.IsMatch("an abb"));
        }

        [Fact]
        public void Regex_IsMatchWithOffset_RespectsAnchors()
        {
            var reg = new Regex("(^hello)|(world)");

            Assert.True(reg.IsMatch("hello there", 0));
            Assert.False(reg.IsMatch("hello there", 1));
            Assert.False(reg.IsMatch("xxxhello", 3));
            Assert.True(reg.IsMatch("hello world", 6));
        }

        [Fact]
        public void Regex_Find_ReturnsValidMatchInfo()
        {
            var reg = new Regex(@"\d{2,4}");

            {
                var match = reg.Find("300");
                Assert.True(match.Matched);
                Assert.Equal(0U, match.Start);
                Assert.Equal(3U, match.End);
            }
            {
                var match = reg.Find("this 1 has 3 numbers in 32 chars");
                Assert.True(match.Matched);
                Assert.Equal(24U, match.Start);
                Assert.Equal(26U, match.End);
            }
        }

        [Fact]
        public void Regex_FindAll_ReturnsIteratorOfMatches()
        {
            var reg = new Regex(@"\d+");

            var matches = reg.FindAll("").ToArray();

            Assert.Equal(0, matches.Length);

            matches = reg.FindAll("4 + 8 = 12").ToArray();

            Assert.Equal(3, matches.Length);
            Assert.Equal(0U, matches[0].Start);
            Assert.Equal(1U, matches[0].End);
            Assert.Equal(4U, matches[1].Start);
            Assert.Equal(5U, matches[1].End);
            Assert.Equal(8U, matches[2].Start);
            Assert.Equal(10U, matches[2].End);
        }

        [Fact]
        public void Regex_FindAllWithEmptuRegex()
        {
            var reg = new Regex("");

            var matches = reg.FindAll("hello").ToArray();

            Assert.Equal(6, matches.Length);
        }

        [Fact]
        public void Regex_CaptureAll_RetursIteratorOfCaptures()
        {
            var reg = new Regex(@"(\d) [+=*/\-] (\d)");

            var caps = reg.CaptureAll("2 * 8 - 9 = 7").ToArray();

            Assert.Equal(2, caps.Length);

            {
                var cap = caps[0];
                Assert.Equal("2 * 8", cap[0].ExtractedString);
            }
            {
                var cap = caps[1];
                Assert.Equal("9", cap[1].ExtractedString);
                Assert.Equal("7", cap[2].ExtractedString);
            }
        }

        [Fact]
        public void Regex_WhenNotAlCapturesAreCaptured_IndividualMatchIsCorrect()
        {
            var reg = new Regex(@"\b(\w)(\d)?(\w)\b");

            using (var captures = reg.Captures("this is a test"))
            {
                Assert.True(captures.Matched);

                Assert.True(captures[0].Matched);
                Assert.True(captures[1].Matched);
                Assert.False(captures[2].Matched);
                Assert.True(captures[3].Matched);
            }
        }

        [Fact]
        public void Regex_GetCaptureName_ReturnsCorrectCapturesIndex()
        {
            var reg = new Regex(@"(?P<foo>[a](?P<bar>\d))(4)(?P<baz>(.{2}))(?:b)(?P<t>7)");

            Assert.Equal(1, reg["foo"]);
            Assert.Equal(2, reg["bar"]);
            Assert.Equal(4, reg["baz"]);
            Assert.Equal(6, reg["t"]);
        }

        [Fact]
        public void Regex_CaptureNamesIter_ReturnsCorrectNames()
        {
            var regex = new Regex(@"(?P<foo>[a](?P<bar>\d))(4)(?P<baz>(.{2}))(?:b)(?P<t>7)");

            var names = regex.CaptureNames().ToArray();

            Assert.Contains("foo", names);
            Assert.Contains("bar", names);
            Assert.Contains("baz", names);
            Assert.Contains("t", names);
            Assert.Equal(4, names.Length);
        }
        
        [Fact]
        public void Regex_IndexIntoCaptureWithGroupName_ReturnsCorrectMatch()
        {
            var reg = new Regex(@"(?P<h>hello) (?P<w>world)");

            var caps = reg.Captures("hello world");
            Assert.Equal("hello", caps["h"].ExtractedString);
            Assert.Equal("world", caps["w"].ExtractedString);
        }

        [Fact]
        public void Regex_FindWithCaptures_ReturnsValidCaptureInfo()
        {
            var dates = new Regex(@"(\d{2})/(\d{2})/(\d{4})");

            using (var captures = dates.Captures("hello on 14/05/2017!"))
            {
                Assert.True(captures.Matched);

                var match = captures[0];
                Assert.True(captures.Matched);
                Assert.Equal(4, captures.Length);

                Assert.Equal(9U, match.Start);
                Assert.Equal(19U, match.End);

                Assert.True(captures[1].Matched);
                Assert.True(captures[2].Matched);
                Assert.True(captures[3].Matched);
            }
        }

        [Fact]
        public void Regex_DateWithNamedCaptures_ExtractsExpectedValues()
        {
            var dates = new Regex(@"(?P<day>\d{2})/(?P<month>\d{2})/(?P<year>\d{4})");

            var haystack = "The first satellite was launched on 04/10/1961";
            var caps = dates.Captures(haystack);
            Assert.True(caps.Matched);
            Assert.Equal("04", caps[dates["day"]].ExtractedString);
            Assert.Equal("10", caps[dates["month"]].ExtractedString);
            Assert.Equal("1961", caps[dates["year"]].ExtractedString);
        }
    }
}
