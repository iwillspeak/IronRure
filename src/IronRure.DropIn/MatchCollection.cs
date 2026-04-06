using System;
using System.Collections;
using System.Collections.Generic;

namespace IronRure.DropIn
{
    /// <summary>
    ///   Represents the set of successful matches found by iteratively applying
    ///   a regular expression to an input string.
    /// </summary>
    public class MatchCollection : ICollection<Match>
    {
        private readonly IList<Match> _matches;

        internal MatchCollection(IList<Match> matches)
        {
            _matches = matches;
        }

        /// <summary>Gets the match at the specified index.</summary>
        public Match this[int i] => _matches[i];

        /// <summary>Gets the number of matches.</summary>
        public int Count => _matches.Count;

        bool ICollection<Match>.IsReadOnly => true;

        /// <inheritdoc />
        public IEnumerator<Match> GetEnumerator() => _matches.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _matches.GetEnumerator();

        void ICollection<Match>.Add(Match item) =>
            throw new NotSupportedException();

        void ICollection<Match>.Clear() =>
            throw new NotSupportedException();

        bool ICollection<Match>.Contains(Match item) => _matches.Contains(item);

        void ICollection<Match>.CopyTo(Match[] array, int arrayIndex) =>
            _matches.CopyTo(array, arrayIndex);

        bool ICollection<Match>.Remove(Match item) =>
            throw new NotSupportedException();
    }
}
