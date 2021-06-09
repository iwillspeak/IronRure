using System;

namespace IronRure
{
    public abstract class RegexIter : IDisposable
    {
        public RegexIter(Regex pattern, byte[] haystack)
        {
            Raw = RureFfi.rure_iter_new(pattern.Raw);
            Pattern = pattern;
            Haystack = haystack;
        }

        protected RegexIterHandle Raw { get; }
        protected Regex Pattern { get; }
        protected byte[] Haystack { get; }

        public void Dispose()
        {
            Raw.Dispose();
        }
    }
}
