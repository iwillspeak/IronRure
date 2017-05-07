using System;
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
    }
}
