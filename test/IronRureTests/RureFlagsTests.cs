using System;
using Xunit;

using IronRure;

namespace IronRureTests
{
    public class RureFlagsTets
    {
        [Fact]
        public void RureFlags_Values_AreCorrectIntegers()
        {
            Assert.Equal(1U, (uint)RureFlags.Casei);
            Assert.Equal(2U, (uint)RureFlags.Multi);
            Assert.Equal(4U, (uint)RureFlags.Dotnl);
            Assert.Equal(8U, (uint)RureFlags.SwapGreed);
            Assert.Equal(16U, (uint)RureFlags.Space);
            Assert.Equal(32U, (uint)RureFlags.Unicode);
        }

        [Fact]
        public void RureFlags_DefaultRegexFlags_AreCorrect()
        {
            Assert.Equal(RureFlags.Unicode, Regex.DefaultFlags);
        }
    }
}
