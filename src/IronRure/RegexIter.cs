using System;

namespace IronRure
{
    public abstract class RegexIter : UnmanagedResource
    {
        protected Regex Pattern { get; }
        protected byte[] Haystack { get; }

        public RegexIter(Regex pattern, byte[] haystack)
            : base(RureFfi.rure_iter_new(pattern.Raw))
        {
            Pattern = pattern;
            Haystack = haystack;
        }

        protected override void Free(IntPtr resource)
        {
            RureFfi.rure_iter_free(resource);
        }
    }
}
