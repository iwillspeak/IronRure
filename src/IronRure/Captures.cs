using System;
using System.Collections;
using System.Collections.Generic;

namespace IronRure;

/// <summary>
/// A set of captures for a given regex.
/// </summary>
public sealed class Captures : IDisposable, IEnumerable<Match>
{
    private readonly byte[] _haystack;
    private readonly Regex _reg;
    private bool _disposed;

    internal Captures(Regex re, byte[] haystack)
    {
        _reg = re;
        _haystack = haystack;
        Raw = RureFfi.rure_captures_new(re.Raw);
    }

    /// <summary>
    /// Gets the Match at a given capture index.
    /// <para>Returns detailed match information for the given capture group.</para>
    /// </summary>
    public Match this[int index]
    {
        get
        {
            var matched = RureFfi.rure_captures_at(Raw, new UIntPtr((uint)index), out var match);
            return new Match(_haystack, matched, checked((uint)match.start), checked((uint)match.end));
        }
    }

    /// <summary>
    /// Gets the match for a given capture name.
    /// </summary>
    public Match this[string group] => this[_reg[group]];

    /// <summary>
    /// Gets the number of groups in the capture set.
    /// </summary>
    public int Length => (int)RureFfi.rure_captures_len(Raw);

    /// <summary>
    /// Overall match status for the pattern.
    /// </summary>
    public bool Matched { get; internal set; }

    /// <summary>
    /// Gets the raw, unmanaged handle to the captures group.
    /// </summary>
    public CapturesHandle Raw { get; }

    /// <summary>
    /// Releases all resources used by this instance.
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        Raw?.Dispose();
        _disposed = true;
    }

    /// <inheritdoc />
    public IEnumerator<Match> GetEnumerator()
    {
        var len = Length;
        for (var i = 0; i < len; i++)
        {
            yield return this[i];
        }
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
