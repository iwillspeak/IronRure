using System;

namespace IronRure
{
    /// <summary>Regex match iterator.</summary>
    public abstract class RegexIter : IDisposable
    {
        /// <summary>Initalise a the regex iterator for the given haystack.</summary>
        protected RegexIter(Regex pattern, byte[] haystack)
        {
            Raw = RureFfi.rure_iter_new(pattern.Raw);
            Pattern = pattern;
            Haystack = haystack;
        }

        /// <summary>The raw handle to the iterator.</summary>
        protected RegexIterHandle Raw { get; }
        
        /// <summary>The pattern that this iterator is using.</summary>
        protected Regex Pattern { get; }

        /// <summary>The haystack being serched.</summary>
        protected byte[] Haystack { get; }

        /// <inheritdoc />
        public void Dispose()
        {
            Raw.Dispose();
        }
    }
}
