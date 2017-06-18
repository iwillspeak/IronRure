using System;
using System.Collections;
using System.Collections.Generic;

namespace IronRure
{
    /// <summary>
    ///   An enumerator of regex captures. Uses the underlying Rure
    ///   regex iterator.
    /// </summary>
    public class CapturesIter : RegexIter, IEnumerator<Captures>
    {
        public CapturesIter(Regex pattern, byte[] haystack)
            : base(pattern, haystack)
        {}

        public Captures Current { get; set; }

        object IEnumerator.Current => (object)Current;

        public bool MoveNext()
        {
            var caps = new Captures(Pattern, Haystack);
            var matched = RureFfi.rure_iter_next_captures(Raw,
                                                          Haystack,
                                                          new UIntPtr((uint)Haystack.Length),
                                                          caps.Raw);

            if (matched)
            {
                caps.Matched = matched;
                Current = caps;
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
