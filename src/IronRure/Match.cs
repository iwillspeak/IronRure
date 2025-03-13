using System.Text;

namespace IronRure;

/// <summary>
///     High-level Match Info
/// </summary>
/// <remarks>Initialise a new <see cref="Match" /> instnace from the constituent parts.</remarks>
public class Match(byte[] haystack, bool matched, uint start, uint end)
{
    /// <summary>Did the pattern match?</summary>
    public bool Matched { get; } = matched;

    /// <summary>The start of the match, in bytes</summary>
    public uint Start { get; } = start;

    /// <summary>The end of the match, in bytes</summary>
    public uint End { get; } = end;

    /// <summary>The C# string that corresponds to the match.</summary>
    public string ExtractedString =>
        Encoding.UTF8.GetString(haystack, (int)Start, (int)(End - Start));
}
