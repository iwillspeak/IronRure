using System;
using System.Collections;
using System.Collections.Generic;

namespace IronRure
{
    /// <summary>
    ///   An enumerator of regex matches. Uses the underlying Rure
    ///   regex iterator.
    /// </summary>
    public class MatchIter : RegexIter, IEnumerator<Match>
    {
        private RureMatch _matchInfo;
        
        /// <summary>Initialise a match iterator.</summary>
        /// <param name="pattern">The pattern to to search with.</param>
        /// <param name="haystack">The haystack to search.</param>
        public MatchIter(Regex pattern, byte[] haystack)
            : base(pattern, haystack)
        {
            _matchInfo = new RureMatch();
        }

        /// <inheritdoc />
        public Match Current { get; set; }

        /// <inheritdoc />
        object IEnumerator.Current => (object)Current;

        /// <inheritdoc />
        public bool MoveNext()
        {
            var matched = RureFfi.rure_iter_next(Raw,
                                                 Haystack,
                                                 new UIntPtr((uint)Haystack.Length),
                                                 out _matchInfo);

            if (matched)
            {
                Current = new Match(Haystack,
                                    matched,
                                    (uint)_matchInfo.start,
                                    (uint)_matchInfo.end);
                return true;
            }

            Current = null;
            return false;
        }

        /// <inheritdoc />
        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
