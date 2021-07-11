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
        /// <summary>Initialise a captures iterator.</summary>
        /// <param name="pattern">The pattern to to search with.</param>
        /// <param name="haystack">The haystack to search.</param>
        public CapturesIter(Regex pattern, byte[] haystack)
            : base(pattern, haystack)
        {}

        /// <inheritdoc />
        public Captures Current { get; set; }

        /// <inheritdoc />
        object IEnumerator.Current => (object)Current;

        /// <inheritdoc />
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

        /// <inheritdoc />
        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
