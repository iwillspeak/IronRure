using System;

namespace IronRure
{
    public struct SetMatch
    {
        public SetMatch(bool matched, bool[] matches)
        {
            Matched = matched;
            Matches = matches;
        }

        /// <summary>
        ///   Did any of the patterns in the set match
        /// </summary>
        public bool Matched { get; }

        /// <summary>
        ///   Match information for each pattern in the set. These
        ///   are in the same order the patterns were passed when
        ///   compiling the set.
        /// </summary>
        public bool[] Matches { get; }
    }
}
