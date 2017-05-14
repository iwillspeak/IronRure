# 🚀⚙️✨ IronRure - Rusty Regexs for .NET ✨⚙️🚀

Rure is the [**Ru**st **R**egular **E**expression crate](https://github.com/rust-lang/regex). This repo aims to provide a set of bindings so that it can be used from .NET.

## Getting Started

IronRure targets the .NET Standard runtime. It's available to install on NuGet:

    > Install-Package IronRure

If you're targeting (Windows or macOS) then it's [batteries included](https://github.com/iwillspeak/IronRure-Batteries). If not then you'll need to supply your own compiled version of `rure`.

## Usage

The simplest operation is to check if a pattern matches anywhere in a given string:

    using IronRure;
    
    var re = new Regex(@"\d+");
    Assert.True(re.IsMatch("I have 1 number"));
    Assert.False(re.IsMatch("I don't"));

All Rust regular expression patterns are unanchored by default. This means that if you want to check if a pattern matches the entire string you'll need to add `^` and `$` yourself:

    var re = new Regex(@"^\w+$");
    
    Assert.True(re.IsMatch("word"));
    Assert.False(re.IsMatch("two words"));

To find the extent of the next match in a string `Regex::Find` can be used:

    var re = new Regex(@"world");
    
    var match = re.Find("hello world");
    Assert.True(match.Matched);
    Assert.Equal(6U, match.Start);
    Assert.Equal(11U, match.End);

### Captures

To get information about the extent of each capture group in a regex the `Regex::Captures` method can be used. This method is slower than `Regex::Find` or `Regex::IsMatch`; only use it if you must retrieve capture information.

    var dates = new Regex(@"(?P<day>\d{2})/(?P<month>\d{2})/(?P<year>\d{4})");

    var haystack = "The first satellite was launched on 04/10/1961";
    var caps = dates.Captures(haystack);
    Assert.True(caps.Matched);
    Assert.Equal("04", caps[dates["day"]].ExtractedString);
    Assert.Equal("10", caps[dates["month"]].ExtractedString);
    Assert.Equal("1961", caps[dates["year"]].ExtractedString);

## Syntax and Semantics

For more information about the pattern syntax see [the underlying Rust crate documentation](https://doc.rust-lang.org/regex/).