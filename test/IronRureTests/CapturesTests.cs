using Xunit;

namespace IronRure.Tests;

public class CapturesTests
{
    [Fact]
    public void CapturesIsEnumerable()
    {
        Regex pattern = new(@"(hello) (\w+)? (world)");
        Captures caps = pattern.Captures("hello world");
        Assert.Equal(4, caps.Length);
    }

    [Fact]
    public void CaptureEnumerableReturnsCorrectCaptures()
    {
        Regex pattern = new(@"(foo)(bar)?");
        Captures caps = pattern.Captures("foo");

        using var capsEnum = caps.GetEnumerator();

        Assert.True(capsEnum.MoveNext());
        Match current = capsEnum.Current;
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
