using System;
using System.Collections;
using System.Collections.Generic;

namespace IronRure;

/// <summary>
///   A set of captures for a given regex.
/// </summary>
public class Captures : IDisposable, IEnumerable<Match>
{
    private readonly byte[] _haystack;
    private readonly Regex _reg;
    private bool _disposed;

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
            bool matched = RureFfi.rure_captures_at(Raw,
                                                   new UIntPtr((uint)index),
                                                   out RureMatch match);
            return new Match(_haystack, matched, checked((uint)match.start), checked((uint)match.end));
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

    /// <summary>Overall match status for the pattern.</summary>
    public bool Matched { get; internal set; }

    /// <summary>
    /// The raw, unmanaged, handle to the captures group
    /// </summary>
    public CapturesHandle Raw { get; }

    /// <inheritdoc />
    public IEnumerator<Match> GetEnumerator()
    {
        int len = Length;
        for (int i = 0; i < len; i++)
        {
            yield return this[i];
        }
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    /// <summary>
    ///   Finalizer
    /// </summary>
    ~Captures()
    {
        Dispose(false);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases the unmanaged resources used by the Captures and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            // Dispose managed resources
            Raw?.Dispose();
        }

        // Dispose unmanaged resources
        _disposed = true;
    }
}
