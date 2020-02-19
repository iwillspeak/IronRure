using System;
using System.Collections;
using System.Collections.Generic;

namespace IronRure
{
    /// <summary>
    ///   A set of captures for a given regex.
    /// </summary>
    public class Captures : IDisposable, IEnumerable<Match>
    {
        private readonly byte[] _haystack;
        private readonly Regex _reg;

        internal Captures(Regex re, byte[] haystack)
            : base()
        {
            _reg = re;
            _haystack = haystack;
            Raw = RureFfi.rure_captures_new(re.Raw);
        }

        /// <summary>
        ///   Get the Match at a given capture index
        ///   <para>
        ///     Returns detailed match information for the given capture group.
        ///   </para>
        /// </summary>
        public Match this[int index]
        {
            get
            {
                var match = new RureMatch();
                var matched = RureFfi.rure_captures_at(Raw,
                                                       new UIntPtr((uint)index),
                                                       out match);
                return new Match(_haystack, matched, (uint)match.start, (uint)match.end);
            }
        }

        /// <summary>
        ///   Get the match for a given capture name.
        /// </summary>
        public Match this[string group] => this[_reg[group]];

        /// <summary>
        ///   The number of groups in the capture set
        /// </summary>
        public int Length => (int)RureFfi.rure_captures_len(Raw);

        public bool Matched { get; internal set; }

        /// <summary>
        /// The raw, unmanaged, handle to the captures group
        /// </summary>
        public CapturesHandle Raw { get; }

        public IEnumerator<Match> GetEnumerator()
        {
            var len = Length;
            for (int i = 0; i < len; i++)
            {
                yield return this[i];
            }
        }
        
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        
        public void Dispose()
        {
            Raw.Dispose();
        }
    }
}
