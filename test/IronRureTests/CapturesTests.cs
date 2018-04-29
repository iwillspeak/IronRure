using Xunit;
using IronRure;
using System.Linq;

namespace IronRure.Tests
{
    public class CapturesTests
    {
        [Fact]
        void CapturesIsEnumerable()
        {
            var pattern = new Regex(@"(hello) (\w+)? (world)");
            var caps = pattern.Captures("hello world");
            Assert.Equal(4, caps.Count());
        }

        [Fact]
        void CaptureEnumerableReturnsCorrectCaptures()
        {
            var pattern = new Regex(@"(foo)(bar)?");
            var caps = pattern.Captures("foo");

            var capsEnum = caps.GetEnumerator();

            Assert.True(capsEnum.MoveNext());
            var current = capsEnum.Current;
            Assert.True(current.Matched);
            Assert.Equal("foo", current.ExtractedString);

            Assert.True(capsEnum.MoveNext());
            current = capsEnum.Current;
            Assert.True(current.Matched);
            Assert.Equal("foo", current.ExtractedString);

            Assert.True(capsEnum.MoveNext());
            current = capsEnum.Current;
            Assert.False(current.Matched);

            Assert.False(capsEnum.MoveNext());
        }
    }
}
