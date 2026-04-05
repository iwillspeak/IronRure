using System;

namespace IronRure;

/// <summary>
/// Regex match iterator.
/// </summary>
public abstract class RegexIter : IDisposable
{
    private bool _disposed;

    /// <summary>
    /// Regex match iterator.
    /// </summary>
    /// <param name="pattern">The pattern for the iterator.</param>
    /// <param name="haystack">The haystack being searched.</param>
    protected RegexIter(Regex pattern, byte[] haystack)
    {
        Raw = pattern?.Raw != null ? RureFfi.rure_iter_new(pattern.Raw) : null;
        Pattern = pattern;
        Haystack = haystack;
    }

    /// <summary>
    /// The raw handle to the iterator.
    /// </summary>
    protected RegexIterHandle Raw { get; }

    /// <summary>
    /// The pattern that this iterator is using.
    /// </summary>
    protected Regex Pattern { get; }

    /// <summary>
    /// The haystack being searched.
    /// </summary>
    protected byte[] Haystack { get; }

    /// <summary>
    /// Disposes the iterator handle.
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        Raw?.Dispose();
        _disposed = true;
        GC.SuppressFinalize(this);
    }
}
