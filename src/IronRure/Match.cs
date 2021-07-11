using System;
using System.Text;

namespace IronRure
{
    /// <summary>
    ///   High-level Match Info
    /// </summary>
    public class Match
    {
        /// <summary>Initialise a new <see cref="Match"/> instnace from the constituent parts.</summary>
        public Match(byte[] haystack, bool matched, uint start, uint end)
        {
            Matched = matched;
            Start = start;
            End = end;
            _haystack = haystack;
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

        private byte[] _haystack;
    }
}
