using System;
using Xunit;

using IronRure;

namespace IronRureTests
{
    public class MatchTests
    {
        [Fact]
        public void Match_CreateWithSuccessfulMatch_ExposesOffsets()
        {
            var match = new Match(true, 1, 3);
            Assert.True(match.Matched);
            Assert.Equal(1U, match.Start);
            Assert.Equal(3U, match.End);
        }

        [Fact]
        public void Match_CreateWithFailedMatch_ExposesStatus()
        {
            var match = new Match(false, 0, 0);
            Assert.False(match.Matched);
        }
    }
}
