using System.Text;

namespace IronRure;

/// <summary>
///     High-level Match Info
/// </summary>
/// <remarks>Initialise a new <see cref="Match" /> instnace from the constituent parts.</remarks>
public class Match
{
    private readonly byte[] _haystack;

    /// <summary>
    ///     High-level Match Info
    /// </summary>
    /// <remarks>Initialise a new <see cref="Match" /> instnace from the constituent parts.</remarks>
    public Match(byte[] haystack, bool matched, uint start, uint end)
    {
        _haystack = haystack;
        Matched = matched;
        Start = start;
        End = end;
    }

    /// <summary>Did the pattern match?</summary>
    public bool Matched { get; }

    /// <summary>The start of the match, in bytes</summary>
    public uint Start { get; }

    /// <summary>The end of the match, in bytes</summary>
    public uint End { get; }

    /// <summary>The C# string that corresponds to the match.</summary>
    public string ExtractedString =>
        Encoding.UTF8.GetString(_haystack, (int)Start, (int)(End - Start));
}
