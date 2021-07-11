using System;

namespace IronRure
{
    /// <summary>Match information for a <see cref="RegexSet" />.</summary>
    public struct SetMatch
    {
        /// <summary>Create a new set <see cref="SetMatch" /> instance.</summary>
        /// <param name="matched">True if any of the expressions matched.</param>
        /// <param name="matches">Match information for each pattern in the set.</param>
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
