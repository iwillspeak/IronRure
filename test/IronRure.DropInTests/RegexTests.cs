using System;
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
        public void MatchWithSimpleWordPattern()
        {
            var regex = new Regex("Hello", RegexOptions.IgnoreCase);

            Assert.True(regex.IsMatch("hello world!"));
            Assert.False(regex.IsMatch("goodbye then"));
            Assert.True(regex.IsMatch("so I said hello to her", 10));
            Assert.False(regex.IsMatch("not a hello in sight", 7));
        }
    }
}
