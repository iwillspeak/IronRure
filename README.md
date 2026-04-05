# 🚀⚙️✨ IronRure - Rusty Regexs for .NET ✨⚙️🚀

Rure is the [**Ru**st **R**egular **E**xpression crate](https://github.com/rust-lang/regex). This repo provides a set of bindings so that it can be used from .NET.

[![Build Status](https://dev.azure.com/iwillspeak/GitHub/_apis/build/status/iwillspeak.IronRure?branchName=refs%2Ftags%2Fv2.0.3)](https://dev.azure.com/iwillspeak/GitHub/_build/latest?definitionId=6&branchName=refs%2Ftags%2Fv2.0.3)

## Getting Started

IronRure targets the .NET Standard runtime. It's available to install on NuGet:

    > Install-Package IronRure

If you're targeting Linux, Windows or macOS then it's [batteries included](https://github.com/iwillspeak/IronRure-Batteries). If not then you'll need to supply your own compiled version of `rure`.

## Tutorials

Walkthroughs and example code to get to know the basic features of IronRure.

* [Re-create `grep` with IronRure](examples/grep/)
* [Filtering User Input with `RegexSet`](examples/filtering/)

## Contributing

IronRure is open source. Pull requests are welcome. See the [Contributing Guidelines][contributing] and [Code of Conduct][coc] for more information.

## Usage

The simplest operation is to check if a pattern matches anywhere in a given string:

```csharp
using IronRure;

var re = new Regex(@"\d+");
Assert.True(re.IsMatch("I have 1 number"));
Assert.False(re.IsMatch("I don't"));
```

All Rust regular expression patterns are unanchored by default. This means that if you want to check if a pattern matches the entire string you'll need to add `^` and `$` yourself:

```csharp
var re = new Regex(@"^\w+$");

Assert.True(re.IsMatch("word"));
Assert.False(re.IsMatch("two words"));
```

To find the extent of the next match in a string `Regex::Find` can be used:

```csharp
var re = new Regex(@"world");

var match = re.Find("hello world");
Assert.True(match.Matched);
Assert.Equal(6U, match.Start);
Assert.Equal(11U, match.End);
```

### Captures

To get information about the extent of each capture group in a regex the `Regex::Captures` method can be used. This method is slower than `Regex::Find` or `Regex::IsMatch`; only use it if you must retrieve capture information.

```csharp
var dates = new Regex(@"(?P<day>\d{2})/(?P<month>\d{2})/(?P<year>\d{4})");

var haystack = "The first satellite was launched on 04/10/1957";
var caps = dates.Captures(haystack);
Assert.True(caps.Matched);
Assert.Equal("04", caps[dates["day"]].ExtractedString);
Assert.Equal("10", caps[dates["month"]].ExtractedString);
Assert.Equal("1957", caps[dates["year"]].ExtractedString);
```

## Performance

In general IronRure out-performs .NET Regex, although milage may vary depending on the exact pattern and text. For more details checkout [the benchmarks](https://github.com/iwillspeak/IronRure/tree/master/bench/Alice).

## Syntax and Semantics

For more information about the pattern syntax see [the underlying Rust crate documentation](https://doc.rust-lang.org/regex/).

## Windows Gotchas

The windows build of [`IronRure-Batteries`](https://github.com/iwillspeak/IronRure-Batteries) requires the `vc140` redistributable to work. This means you need to have Visual Studio 2015 or the [Visual Studio 2015 C++ Runtime](https://www.microsoft.com/en-gb/download/details.aspx?id=48145) installed for it to load. If not you'll get an exception which claims that `rure.dll` can't be found.

[contributing]: CONTRIBUTING.md
[coc]: CODE_OF_CONDUCT.md
