using System;

namespace IronRure
{
    /// <summary>
    ///   A set of captures for a given regex.
    /// </summary>
    public class Captures : UnmanagedResource
    {
        internal Captures(Regex re)
            : base(RureFfi.rure_captures_new(re.Raw))
        {}

        protected override void Free(IntPtr resource)
        {
            RureFfi.rure_captures_free(resource);
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
                return new Match(matched, (uint)match.start, (uint)match.end);
            }
        }

        /// <summary>
        ///   The number of groups in the capture set
        /// </summary>
        public int Length => (int)RureFfi.rure_captures_len(Raw);

        public bool Matched { get; internal set; }
    }
}
