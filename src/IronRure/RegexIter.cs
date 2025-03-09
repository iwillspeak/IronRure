using System;

namespace IronRure;

/// <summary>Regex match iterator.</summary>
/// <remarks>Initalise a the regex iterator for the given haystack.</remarks>
public abstract class RegexIter(Regex pattern, byte[] haystack) : IDisposable
{

    /// <summary>The raw handle to the iterator.</summary>
    protected RegexIterHandle Raw { get; } = RureFfi.rure_iter_new(pattern.Raw);

    /// <summary>The pattern that this iterator is using.</summary>
    protected Regex Pattern { get; } = pattern;

    /// <summary>The haystack being serched.</summary>
    protected byte[] Haystack { get; } = haystack;

    /// <inheritdoc />
    public void Dispose()
    {
        Raw.Dispose();
    }
}
