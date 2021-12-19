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
    }
}
