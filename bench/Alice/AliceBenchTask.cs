﻿using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using IronRure;
using Regex = System.Text.RegularExpressions.Regex;

namespace Alice;

[MemoryDiagnoser]
[RPlotExporter]
[RankColumn]
[AsciiDocExporter]
[MarkdownExporter]
public class AliceBenchTask
{
    private static readonly string AliceFilename = "Alice's_Adventures_in_Wonderland.txt";
    private static string text;

    private Regex dotnetRegex;
    private Regex dotnetRegexNoCompile;
    private IronRure.Regex rustRegex;
    private IronRure.Regex rustRegexUnicode;
#if IncludeRe2
    private IronRe2.Regex re2Regex;
#endif

    [GlobalSetup]
    public void GlobalSetup()
    {
        text = File.ReadAllText(AliceFilename, Encoding.UTF8);

        InitializeRegex();
    }

    [Params(
        @"\b(\w+)-legged",
        //@"Alice",
        //@"^Alice",
        //@"Alice$",
        //@"\d+",
        //@"a[^x]{20}b",
        //@"\w+@\w+.\w+",
        //"\"[^\"]+\"",
        //"\"[^\"]{0,30}[?!.]\"",
        //"\"[^\"]+\"\\s+said",
        //@"(\*\s+){4}\*",
        //@"[a-q][^u-z]{13}x",
        @"[a-zA-Z]+ing",
        @"Alice|Adventure",
        //@"Alice|Hatter|Cheshire|Dinah",
        @".{0,3}(Alice|Hatter|Cheshire|Dinah)",
        //@"zqj",
        //@"aei",
        //"(?i)the",
        //"^.{16,20}$",
        //".+",
        //"(?s).+",
        "Alice.{0,25}Hatter|Hatter.{0,25}Alice",
        "([A-Za-z]lice|[A-Za-z]heshire)[^a-zA-Z]"
        //".*"
    )]
    public string Pattern;

    //[IterationSetup]
    public void InitializeRegex()
    {
        dotnetRegex = new Regex(
            Pattern,
            RegexOptions.IgnoreCase | RegexOptions.Compiled);
        dotnetRegexNoCompile = new Regex(
            Pattern,
            RegexOptions.IgnoreCase);

#if IncludeRe2
        re2Regex = new IronRe2.Regex(Pattern, new IronRe2.Options { CaseSensitive = false });
#endif

        rustRegex = new IronRure.Regex(Pattern, RureFlags.Casei);
        rustRegexUnicode = new IronRure.Regex(Pattern, RureFlags.Casei | RureFlags.Unicode);
    }

    [Benchmark(Baseline = true)]
    public void DotnetRegex()
    {
        dotnetRegex.Matches(text).EnsureEnumerated();
    }

    [Benchmark]
    public void DotnetRegexNoCompile()
    {
        dotnetRegexNoCompile.Matches(text).EnsureEnumerated();
    }

#if IncludeRe2
    [Benchmark]
    public void Re2Regex() => re2Regex.FindAll(text).EnsureEnumerated();
#endif

    [Benchmark]
    public void RustRegex()
    {
        rustRegex.FindAll(text).EnsureEnumerated();
    }

    [Benchmark]
    public void RustRegexUnicode()
    {
        rustRegexUnicode.FindAll(text).EnsureEnumerated();
    }
}

public static class Extensions
{
    public static void EnsureEnumerated(this IEnumerable e)
    {
        if (e == null)
        {
            return;
        }

        foreach (var _ in e)
        {
        }
    }
}
