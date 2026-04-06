using System;
using System.Text;

namespace IronRure.DropIn
{
    /// <summary>
    ///   Represents the results from a single regular expression match.
    /// </summary>
    public class Match : Group
    {
        private readonly Regex _regex;
        private readonly string _haystack;

        /// <summary>
        ///   The byte offset in the UTF-8 haystack at which the next search
        ///   should begin (i.e. one past the end of the current match in bytes).
        /// </summary>
        private readonly int _nextByteOffset;

        internal Match(bool success, string value, int index, int length,
                       GroupCollection groups, Regex regex, string haystack,
                       int nextByteOffset)
            : base(success, value, index, length)
        {
            Groups = groups;
            _regex = regex;
            _haystack = haystack;
            _nextByteOffset = nextByteOffset;
        }

        /// <summary>Creates an empty, unsuccessful match.</summary>
        private Match()
            : base(false, string.Empty, 0, 0)
        {
            Groups = new GroupCollection(
                new Group[] { this },
                new System.Collections.Generic.Dictionary<string, int>());
        }

        /// <summary>
        ///   Returns an empty, unsuccessful match (equivalent to a failed match).
        /// </summary>
        public static readonly Match Empty = new Match();

        /// <summary>
        ///   Gets a collection of groups matched by the regular expression.
        /// </summary>
        public GroupCollection Groups { get; }

        /// <summary>
        ///   Returns a new <see cref="Match" /> object with the results for the
        ///   next match, starting at the position at which the last match ended.
        /// </summary>
        public Match NextMatch()
        {
            if (!Success || _regex == null)
                return Empty;
            return _regex.MatchAtByteOffset(_haystack, _nextByteOffset);
        }

        /// <summary>
        ///   Returns the expansion of the specified replacement pattern.
        /// </summary>
        public virtual string Result(string replacement)
        {
            return Regex.ExpandReplacement(replacement, this);
        }

        /// <summary>
        ///   Returns a <see cref="Match" /> equivalent to the supplied one,
        ///   suitable for sharing between threads.
        /// </summary>
        public static Match Synchronized(Match inner) => inner;
    }
}
