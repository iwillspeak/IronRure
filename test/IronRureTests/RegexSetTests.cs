using System;
using Xunit;

namespace IronRure.Tests;

public class RegexSetTests
{
    [Fact]
    public void RegexSet_CreateWithSingleRegex_Succeeds()
    {
        RegexSet set = new(".+");
        Assert.NotNull(set);
    }

    [Fact]
    public void RegexSet_CreateWithMultiplePatterns_Succeeds()
    {
        RegexSet set = new("hel+o", "world", "0x[a-zA-Z0-9]{4,8}");
        Assert.NotNull(set);
    }

    [Fact]
    public void RegexSet_AsIDisposable_ImplementsInterface()
    {
        RegexSet set = new("");
        var dispo = set as IDisposable;

        Assert.NotNull(dispo);
    }

    [Fact]
    public void RegexSet_IsMatch_ReturnsTrueIfAnyRegexMatches()
    {
        using RegexSet regs = new("fo+", "[0-9]+");
        Assert.True(regs.IsMatch("fooooo"));
        Assert.True(regs.IsMatch("bar 1"));
        Assert.False(regs.IsMatch("no match here"));
    }

    [Fact]
    public void RegexSet_Matches_DigitsAndLetters()
    {
        using RegexSet regs = new("[a-z]+", "[0-9]+");
        Assert.True(regs.IsMatch("hello"));
        Assert.True(regs.IsMatch("1234"));
        Assert.True(regs.IsMatch("l33t"));
        var match = regs.Matches("hello");
        Assert.Equal([true, false], match.Matches);
        match = regs.Matches("123");
        Assert.Equal([false, true], match.Matches);
    }

    [Fact]
    public void RegexSet_Matches_ExposesMatchingPatterns()
    {
        using RegexSet regs = new("hel+o", "regex", "world");
        var match = regs.Matches("hello world");
        Assert.True(match.Matched);
        Assert.True(match.Matches[0]);
        Assert.False(match.Matches[1]);
        Assert.True(match.Matches[2]);
        Assert.Equal([true, false, true], match.Matches);
    }

    [Fact]
    public void RegexSet_MatchWithFlags_RespectsFlags()
    {
        using RegexSet regs = new(RureFlags.Casei, "a", "e", "i", "o", "u");
        Assert.True(regs.IsMatch("a"));
        Assert.True(regs.IsMatch("E"));
        var match = regs.Matches("Union");
        Assert.True(match.Matched);
        Assert.Equal([false, false, true, true, true], match.Matches);
    }

    [Fact]
    public void RegexSet_Match_WithOptions()
    {
        var opts = new Options()
            .WithDfaSize(100);
        using RegexSet regs = new(Regex.DefaultFlags, opts, "foo+", "ba?r");
        Assert.True(regs.IsMatch("foooooooooooo"));
        var match = regs.Matches("br");
        Assert.True(match.Matched);
        Assert.Equal([false, true], match.Matches);
    }
}
