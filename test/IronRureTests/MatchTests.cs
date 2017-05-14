using System;
using System.Text;
using Xunit;

using IronRure;

namespace IronRureTests
{
    public class MatchTests
    {
        [Fact]
        public void Match_CreateWithSuccessfulMatch_ExposesOffsets()
        {
            var match = new Match(Encoding.UTF8.GetBytes("test"), true, 1, 3);
            Assert.True(match.Matched);
            Assert.Equal(1U, match.Start);
            Assert.Equal(3U, match.End);
        }

        [Fact]
        public void Match_CreateWithFailedMatch_ExposesStatus()
        {
            var match = new Match(Array.Empty<byte>(), false, 0, 0);
            Assert.False(match.Matched);
        }

        [Fact]
        public void Match_ExtractedText_IsCorrectSubstring()
        {
            var haystack = Encoding.UTF8.GetBytes("hello wørld");
            var match = new Match(haystack, true, 0, 5);
            Assert.Equal("hello", match.ExtractedString);

            match = new Match(haystack, true, 6, 12);
            Assert.Equal("wørld", match.ExtractedString);
        }
    }
}
