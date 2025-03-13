using System;
using System.Collections.Generic;
using System.Text;

namespace IronRure;

/// <summary>
///     Rust Regex
///     <para>
///         Managed wrapper around the rust regex class.
///     </para>
/// </summary>
public partial class Regex : IDisposable
{
    private bool _disposed;

    /// <summary>
    ///     Create a new regex instance from the given pattern.
    /// </summary>
    /// <param name="pattern">The regular expression pattern.</param>
    public Regex(string pattern)
        : this(pattern, new OptionsHandle(), (uint)DefaultFlags)
    {
    }

    /// <summary>
    ///     Create a new regex instance from the given pattern, and with the
    ///     given regex options applied.
    /// </summary>
    /// <param name="pattern">The regular expression pattern.</param>
    /// <param name="opts">The options to apply to the regular expression.</param>
    public Regex(string pattern, Options opts)
        : this(pattern, opts.Raw, (uint)DefaultFlags)
    {
    }

    /// <summary>
    ///     Create a new regex instance from the given pattern and flags.
    /// </summary>
    /// <param name="pattern">The regular expression pattern.</param>
    /// <param name="flags">The flags to apply to the regular expression.</param>
    public Regex(string pattern, RureFlags flags)
        : this(pattern, new OptionsHandle(), (uint)flags)
    {
    }

    /// <summary>
    ///     Create a new regex instance from the given pattern, with the given
    ///     options applied and with the given flags enabled.
    /// </summary>
    /// <param name="pattern">The regular expression pattern.</param>
    /// <param name="opts">The options to apply to the regular expression.</param>
    /// <param name="flags">The flags to apply to the regular expression.</param>
    public Regex(string pattern, Options opts, RureFlags flags)
        : this(pattern, opts.Raw, (uint)flags)
    {
    }

    private Regex(string pattern, OptionsHandle options, uint flags)
    {
        Raw = Compile(pattern, options, flags);
    }

    /// <summary>
    ///     Gets the raw RegexHandle.
    /// </summary>
    internal RegexHandle Raw { get; private set; }

    /// <summary>
    ///     Get Capture Index from Name
    /// </summary>
    /// <param name="capture">The name of the capture group.</param>
    /// <returns>The index of the named capture group.</returns>
    public int this[string capture]
    {
        get
        {
            return Raw != null ? RureFfi.rure_capture_name_index(Raw, Encoding.UTF8.GetBytes(capture)) : 0;
        }
    }

    /// <summary>
    ///     The default flags for the regex
    /// </summary>
    public static RureFlags DefaultFlags => RureFlags.Unicode;

    /// <summary>
    ///     Compiles the given regex and returns the unmanaged pointer to it.
    /// </summary>
    /// <param name="pattern">The regular expression pattern to compile.</param>
    /// <param name="options">The options to apply to the regular expression.</param>
    /// <param name="flags">The flags to apply to the regular expression.</param>
    /// <returns>A handle to the compiled regex.</returns>
    /// <exception cref="RegexCompilationException">Thrown when the regex fails to compile.</exception>
    private static RegexHandle Compile(string pattern, OptionsHandle options, uint flags)
    {
        // Convert the pattern to a utf-8 encoded string.
        var patBytes = Encoding.UTF8.GetBytes(pattern);

        using var err = RureFfi.rure_error_new();
        // Compile the regex. We get back a raw handle to the underlying
        // Rust object.
        var raw = RureFfi.rure_compile(
            patBytes,
            new UIntPtr((uint)patBytes.Length),
            flags,
            options,
            err);

        // If the regex failed to compile find out what the problem was.
        if (raw.IsInvalid)
        {
            throw new RegexCompilationException(err.Message ?? "Error retrieving the error");
        }

        return raw;
    }

    /// <summary>
    ///     Test if this Regex matches <paramref name="haystack" />
    /// </summary>
    /// <param name="haystack">The string to search for this pattern</param>
    /// <returns>True if the pattern matches, false otherwise.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the Regex has been disposed.</exception>
    public bool IsMatch(string haystack)
    {
        return IsMatch(haystack, 0);
    }

    /// <summary>
    ///     Test if this Regex matches <paramref name="haystack" />, starting
    ///     at the given <paramref name="offset" />.
    /// </summary>
    /// <param name="haystack">The string to search for this pattern</param>
    /// <param name="offset">The offset to start searching at</param>
    /// <returns>True if the pattern matches, false otherwise.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the Regex has been disposed.</exception>
    public bool IsMatch(string haystack, uint offset)
    {
        ThrowIfDisposed();
        var haystackBytes = Encoding.UTF8.GetBytes(haystack);

        return RureFfi.rure_is_match(
            Raw ?? throw new InvalidOperationException(), haystackBytes,
            new UIntPtr((uint)haystackBytes.Length),
            new UIntPtr(offset));
    }

    /// <summary>
    ///     Find the extent of the first match.
    /// </summary>
    /// <param name="haystack">The byte array to search for this pattern</param>
    /// <param name="offset">The offset to start searching at</param>
    /// <returns>A Match object containing information about the match.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the Regex has been disposed.</exception>
    public Match Find(byte[] haystack, uint offset)
    {
        ThrowIfDisposed();

        var matched = RureFfi.rure_find(
            Raw ?? throw new InvalidOperationException(), haystack,
            new UIntPtr((uint)haystack.Length),
            new UIntPtr(offset),
            out var matchInfo);

        return new Match(haystack, matched, checked((uint)matchInfo.start), checked((uint)matchInfo.end));
    }

    /// <summary>
    ///     Find the extent of the first match.
    /// </summary>
    /// <param name="haystack">The string to search for this pattern</param>
    /// <param name="offset">The offset to start searching at</param>
    /// <returns>A Match object containing information about the match.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the Regex has been disposed.</exception>
    public Match Find(string haystack, uint offset)
    {
        ThrowIfDisposed();
        var haystackBytes = Encoding.UTF8.GetBytes(haystack);
        return Find(haystackBytes, offset);
    }

    /// <summary>
    ///     Find the extent of the first match.
    /// </summary>
    /// <param name="haystack">The string to search for this pattern</param>
    /// <returns>A Match object containing information about the match.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the Regex has been disposed.</exception>
    public Match Find(string haystack)
    {
        return Find(haystack, 0);
    }

    /// <summary>
    ///     Find All Matches
    ///     <para>Returns an iterator over each match in the pattern</para>
    /// </summary>
    /// <param name="haystack">The byte array to search for this pattern</param>
    /// <returns>A list of Match objects.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the Regex has been disposed.</exception>
    public IEnumerable<Match> FindAll(byte[] haystack)
    {
        ThrowIfDisposed();
        using MatchIter iter = new(this, haystack);
        while (iter.MoveNext())
        {
            yield return iter.Current;
        }
    }

    /// <summary>
    ///     Find All Matches
    ///     <para>Returns an iterator over each match in the pattern</para>
    /// </summary>
    /// <param name="haystack">The string to search for this pattern</param>
    /// <returns>A list of Match objects.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the Regex has been disposed.</exception>
    public IEnumerable<Match> FindAll(string haystack)
    {
        ThrowIfDisposed();
        var haystackBytes = Encoding.UTF8.GetBytes(haystack);
        return FindAll(haystackBytes);
    }

    /// <summary>
    ///     Get an enumeration of all the named captures in this pattern.
    /// </summary>
    /// <returns>A list of capture names.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the Regex has been disposed.</exception>
    public IEnumerable<string> CaptureNames()
    {
        ThrowIfDisposed();
        return new CaptureNamesEnumerable(this);
    }

    /// <summary>
    ///     Captures - Find the extent of the capturing groups in the pattern
    ///     in a given haystack.
    /// </summary>
    /// <param name="haystack">The byte array to search for this pattern</param>
    /// <param name="offset">The offset to start searching at</param>
    /// <returns>A Captures object containing information about the captures.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the Regex has been disposed.</exception>
    public Captures Captures(byte[] haystack, uint offset)
    {
        ThrowIfDisposed();
        Captures caps = new(this, haystack);
        caps.Matched = RureFfi.rure_find_captures(Raw,
            haystack,
            new UIntPtr((uint)haystack.Length),
            new UIntPtr(offset),
            caps.Raw);
        return caps;
    }

    /// <summary>
    ///     Captures - Find the extent of the capturing groups in the pattern
    ///     in a given haystack.
    /// </summary>
    /// <param name="haystack">The byte array to search for this pattern</param>
    /// <returns>A Captures object containing information about the captures.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the Regex has been disposed.</exception>
    public Captures Captures(byte[] haystack)
    {
        return Captures(haystack, 0);
    }

    /// <summary>
    ///     Captures - Find the extent of the capturing groups in the pattern
    ///     in a given haystack.
    /// </summary>
    /// <param name="haystack">The string to search for this pattern</param>
    /// <param name="offset">The offset to start searching at</param>
    /// <returns>A Captures object containing information about the captures.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the Regex has been disposed.</exception>
    public Captures Captures(string haystack, uint offset)
    {
        ThrowIfDisposed();
        var haystackBytes = Encoding.UTF8.GetBytes(haystack);
        return Captures(haystackBytes, offset);
    }

    /// <summary>
    ///     Captures - Find the extent of the capturing groups in the pattern
    ///     in a given haystack.
    /// </summary>
    /// <param name="haystack">The string to search for this pattern</param>
    /// <returns>A Captures object containing information about the captures.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the Regex has been disposed.</exception>
    public Captures Captures(string haystack)
    {
        return Captures(haystack, 0);
    }

    /// <summary>
    ///     Capture All Matches
    ///     <para>
    ///         Returns an iterator containing the capture information
    ///         for each match of the pattern.
    ///     </para>
    /// </summary>
    /// <param name="haystack">The byte array to search for this pattern</param>
    /// <returns>A list of Captures objects.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the Regex has been disposed.</exception>
    public IEnumerable<Captures> CaptureAll(byte[] haystack)
    {
        ThrowIfDisposed();
        using CapturesIter iter = new(this, haystack);
        while (iter.MoveNext())
        {
            yield return iter.Current;
        }
    }

    /// <summary>
    ///     Capture All Matches
    ///     <para>
    ///         Returns an iterator containing the capture information
    ///         for each match of the pattern.
    ///     </para>
    /// </summary>
    /// <param name="haystack">The string to search for this pattern</param>
    /// <returns>A list of Captures objects.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the Regex has been disposed.</exception>
    public IEnumerable<Captures> CaptureAll(string haystack)
    {
        ThrowIfDisposed();
        var haystackBytes = Encoding.UTF8.GetBytes(haystack);
        return CaptureAll(haystackBytes);
    }

    /// <summary>
    ///     Throws an ObjectDisposedException if this Regex has been disposed.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the Regex has been disposed.</exception>
    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(Regex));
    }

    /// <summary>
    ///     Releases the unmanaged resources used by the Regex and optionally releases the managed resources.
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        // Dispose unmanaged resources safely.
        if (Raw is { IsInvalid: false })
        {
            Raw.Dispose();
            Raw = null;
        }
        _disposed = true;
        GC.SuppressFinalize(this);
    }
}
