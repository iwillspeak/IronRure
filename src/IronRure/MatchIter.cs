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
        
        public MatchIter(Regex pattern, byte[] haystack)
            : base(pattern, haystack)
        {
            _matchInfo = new RureMatch();
        }

        public Match Current { get; set; }

        object IEnumerator.Current => (object)Current;

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

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
