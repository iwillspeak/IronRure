using System;

namespace IronRure
{
    /// <summary>
    ///   High-level Match Info
    /// </summary>
    public class Match
    {
        public Match(bool matched, int start, int end)
        {
            Matched = matched;
            Start = start;
            End = end;
        }

        /// <summary>Did the pattern match?</summary>
        public bool Matched { get; }

        /// <summary>The start of the match, in bytes</summary>
        public int Start { get; }

        /// <summary>The end of the match, in bytes</summary>
        public int End { get; }
    }
}