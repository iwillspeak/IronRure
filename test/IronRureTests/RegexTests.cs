using System;
using System.Linq;
using Xunit;

namespace IronRure.Tests;

public class RegexTests
{
    [Fact]
    public void Regex_CreateWithEmptyPattern_Succeeds()
    {
        Regex regex = new("");
        Assert.NotNull(regex);
    }

    [Fact]
    public void Regex_AsIDisposable_ImplementsInterface()
    {
        Regex reg = new(".*");
        IDisposable dispo = reg as IDisposable;
        Assert.NotNull(dispo);
    }

    [Fact]
    public void Regex_CreateWithOptions_Succeeds()
    {
        //This used to work with 512, why did this change Will? (MAybe it's cause it's Windows) Will adjust
        using Regex reg = new(@"\w+", new Options().WithSize(65536).WithDfaSize(65536));
        Assert.True(reg.IsMatch("hello"));
        Assert.False(reg.IsMatch("!@£$"));
    }

    [Fact]
    public void Regex_CreateWithFlags_Succeeds()
    {
        using Regex reg = new("", new Options(), Regex.DefaultFlags);
        Assert.True(reg.IsMatch(""));
    }

    [Fact]
    public void Regex_CreateWithFlagsOnly_Succeeds()
    {
        using Regex reg = new(@"Δ", RureFlags.Unicode | RureFlags.Casei);
        Assert.True(reg.IsMatch("δ"));
    }

    [Fact]
    public void Regex_CreateWithInvalidPattern_ThrowsException()
    {
        Assert.Throws<RegexCompilationException>(() => new Regex(")"));
    }

    [Fact]
    public void Regex_IsMatch_ReturnsTrueWhenPatternMatches()
    {
        Regex reg = new("(abb)|c");

        Assert.False(reg.IsMatch("world"));
        Assert.True(reg.IsMatch("circle"));
        Assert.True(reg.IsMatch("abb circle"));
        Assert.True(reg.IsMatch("an abb"));
    }

    [Fact]
    public void Regex_IsMatchWithOffset_RespectsAnchors()
    {
        Regex reg = new("(^hello)|(world)");

        Assert.True(reg.IsMatch("hello there", 0));
        Assert.False(reg.IsMatch("hello there", 1));
        Assert.False(reg.IsMatch("xxxhello", 3));
        Assert.True(reg.IsMatch("hello world", 6));
    }

    [Fact]
    public void Regex_Find_ReturnsValidMatchInfo()
    {
        Regex reg = new(@"\d{2,4}");

        {
            Match match = reg.Find("300");
            Assert.True(match.Matched);
            Assert.Equal(0U, match.Start);
            Assert.Equal(3U, match.End);
        }
        {
            Match match = reg.Find("this 1 has 3 numbers in 32 chars");
            Assert.True(match.Matched);
            Assert.Equal(24U, match.Start);
            Assert.Equal(26U, match.End);
        }
    }

    [Fact]
    public void Regex_FindAll_ReturnsIteratorOfMatches()
    {
        Regex reg = new(@"\d+");

        var matches = reg.FindAll("").ToArray();

        Assert.Empty(matches);

        matches = [.. reg.FindAll("4 + 8 = 12")];

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
        Regex reg = new("");

        var matches = reg.FindAll("hello").ToArray();

        Assert.Equal(6, matches.Length);
    }

    [Fact]
    public void Regex_CaptureAll_RetursIteratorOfCaptures()
    {
        Regex reg = new(@"(\d) [+=*/\-] (\d)");

        var caps = reg.CaptureAll("2 * 8 - 9 = 7").ToArray();

        Assert.Equal(2, caps.Length);

        {
            Captures cap = caps[0];
            Assert.Equal("2 * 8", cap[0].ExtractedString);
        }
        {
            Captures cap = caps[1];
            Assert.Equal("9", cap[1].ExtractedString);
            Assert.Equal("7", cap[2].ExtractedString);
        }
    }

    [Fact]
    public void Regex_WhenNotAlCapturesAreCaptured_IndividualMatchIsCorrect()
    {
        Regex reg = new(@"\b(\w)(\d)?(\w)\b");

        using Captures captures = reg.Captures("this is a test");
        Assert.True(captures.Matched);

        Assert.True(captures[0].Matched);
        Assert.True(captures[1].Matched);
        Assert.False(captures[2].Matched);
        Assert.True(captures[3].Matched);
    }

    [Fact]
    public void Regex_GetCaptureName_ReturnsCorrectCapturesIndex()
    {
        Regex reg = new(@"(?P<foo>[a](?P<bar>\d))(4)(?P<baz>(.{2}))(?:b)(?P<t>7)");

        Assert.Equal(1, reg["foo"]);
        Assert.Equal(2, reg["bar"]);
        Assert.Equal(4, reg["baz"]);
        Assert.Equal(6, reg["t"]);
    }

    [Fact]
    public void Regex_CaptureNamesIter_ReturnsCorrectNames()
    {
        Regex regex = new(@"(?P<foo>[a](?P<bar>\d))(4)(?P<baz>(.{2}))(?:b)(?P<t>7)");

        string[] names = [.. regex.CaptureNames()];

        Assert.Contains("foo", names);
        Assert.Contains("bar", names);
        Assert.Contains("baz", names);
        Assert.Contains("t", names);
        Assert.Equal(4, names.Length);
    }
    
    [Fact]
    public void Regex_IndexIntoCaptureWithGroupName_ReturnsCorrectMatch()
    {
        Regex reg = new(@"(?P<h>hello) (?P<w>world)");

        Captures caps = reg.Captures("hello world");
        Assert.Equal("hello", caps["h"].ExtractedString);
        Assert.Equal("world", caps["w"].ExtractedString);
    }

    [Fact]
    public void Regex_FindWithCaptures_ReturnsValidCaptureInfo()
    {
        Regex dates = new(@"(\d{2})/(\d{2})/(\d{4})");

        using Captures captures = dates.Captures("hello on 14/05/2017!");
        Assert.True(captures.Matched);

        Match match = captures[0];
        Assert.True(captures.Matched);
        Assert.Equal(4, captures.Length);

        Assert.Equal(9U, match.Start);
        Assert.Equal(19U, match.End);

        Assert.True(captures[1].Matched);
        Assert.True(captures[2].Matched);
        Assert.True(captures[3].Matched);
    }

    [Fact]
    public void Regex_DateWithNamedCaptures_ExtractsExpectedValues()
    {
        Regex dates = new(@"(?P<day>\d{2})/(?P<month>\d{2})/(?P<year>\d{4})");

        const string haystack = "The first satellite was launched on 04/10/1961";
        Captures caps = dates.Captures(haystack);
        Assert.True(caps.Matched);
        Assert.Equal("04", caps[dates["day"]].ExtractedString);
        Assert.Equal("10", caps[dates["month"]].ExtractedString);
        Assert.Equal("1961", caps[dates["year"]].ExtractedString);
    }

    [Fact]
    public void Regex_ReplaceWithLiteralString_ReplacesFirstMatch()
    {
        Regex numbers = new(@"\d+");

        const string haystack = "7 ate 9 because it wanted 3 square meals";

        Assert.Equal("* ate 9 because it wanted 3 square meals", numbers.Replace(haystack, "*"));
    }

    [Fact]
    public void Regex_ReplaceAllWithLiteralString_ReplacesAllMatches()
    {
        Regex numbers = new(@"\d+");

        const string haystack = "1, 2, 3 and 4";
        Assert.Equal("#, #, # and #", numbers.ReplaceAll(haystack, "#"));
    }

    [Fact]
    public void Regex_ReplaceWithCount_ReplacesOnlyRequestedMatches()
    {
        Regex words = new(@"\b\w+\b");

        const string haystack = "super six 4, this is super six four.";
        Assert.Equal("$ $ $, this is super six four.", words.Replace(haystack, "$", 3));
    }

    [Fact]
    public void Regex_ReplaceWithComputedReplacement_InsertsCorrectReplacement()
    {
        Regex longWords = new(@"\b\w{1,5}\b");

        const string haystack = "hello replacing world!";

        Assert.Equal("##### replacing #####!",
                     longWords.ReplaceAll(haystack,
                                          m => new string('#', (int)(m.End - m.Start))));
    }
}
