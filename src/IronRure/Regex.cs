using System;
using System.Collections.Generic;
using System.Text;

namespace IronRure;

/// <summary>
///   Rust Regex
///
///   <para>
///     Managed wrapper around the rust regex class.
///   </para>
/// </summary>
public partial class Regex : IDisposable
{
    private bool _disposed;

    /// <summary>
    ///   Create a new regex instance from the given pattern.
    /// </summary>
    public Regex(string pattern)
        : this(pattern, new OptionsHandle(), (uint)DefaultFlags)
    { }

    /// <summary>
    ///   Create a new regex instance from the given pattern, and with the
    ///   given regex options applied.
    /// </summary>
    public Regex(string pattern, Options opts)
        : this(pattern, opts.Raw, (uint)DefaultFlags)
    { }

    /// <summary>
    ///   Create a new regex instance from the given pattern and flags.
    /// </summary>
    public Regex(string pattern, RureFlags flags)
        : this(pattern, new OptionsHandle(), (uint)flags)
    { }

    /// <summary>
    ///   Create a new regex instance from the given pattern, with the given
    ///   options applied and with the given flags enabled.
    /// </summary>
    public Regex(string pattern, Options opts, RureFlags flags)
        : this(pattern, opts.Raw, (uint)flags)
    { }

    private Regex(string pattern, OptionsHandle options, uint flags)
    {
        Raw = Compile(pattern, options, flags);
    }

    internal RegexHandle Raw { get; }

    /// <summary>
    ///   Compiles the given regex and returns the unmanaged pointer to it.
    /// </summary>
    private static RegexHandle Compile(string pattern, OptionsHandle options, uint flags)
    {
        // Convert the pattern to a utf-8 encoded string.
        byte[] patBytes = Encoding.UTF8.GetBytes(pattern);

        using ErrorHandle err = RureFfi.rure_error_new();
        // Compile the regex. We get back a raw handle to the underlying
        // Rust object.
        RegexHandle raw = RureFfi.rure_compile(
            patBytes,
            new UIntPtr((uint)patBytes.Length),
            flags,
            options,
            err);

        // If the regex failed to compile find out what the problem was.
        if (raw.IsInvalid)
            throw new RegexCompilationException(err.Message);

        return raw;
    }

    /// <summary>
    ///   Test if this Regex matches <paramref name="haystack" />
    /// </summary>
    /// <param name="haystack">The UTF8 bytes to search for this pattern</param>
    public bool IsMatch(string haystack) => IsMatch(haystack, 0);

    /// <summary>
    ///   Test if this Regex matches <paramref name="haystack" />, starting
    ///   at the given <paramref name="offset" />.
    /// </summary>
    /// <param name="haystack">The string to search for this pattern</param>
    /// <param name="offset">The offset to start searching at</param>
    public bool IsMatch(string haystack, uint offset)
    {
        byte[] haystackBytes = Encoding.UTF8.GetBytes(haystack);

        return RureFfi.rure_is_match(
            Raw, haystackBytes,
            new UIntPtr((uint)haystackBytes.Length),
            new UIntPtr(offset));
    }

    /// <summary>
    ///   Find the extent of the first match.
    /// </summary>
    /// <param name="offset">The offset to start searching at</param>
    /// <param name="haystack">The string to search for this pattern</param>
    public Match Find(byte[] haystack, uint offset)
    {
        bool matched = RureFfi.rure_find(
            Raw, haystack,
            new UIntPtr((uint)haystack.Length),
            new UIntPtr(offset),
            out RureMatch matchInfo);

        return new Match(haystack, matched, checked((uint)matchInfo.start), checked((uint)matchInfo.end));
    }

    /// <summary>
    ///   Find the extent of the first match.
    /// </summary>
    /// <param name="haystack">The string to search for this pattern</param>
    /// <param name="offset">The offset to start searching at</param>
    public Match Find(string haystack, uint offset)
    {
        byte[] haystackBytes = Encoding.UTF8.GetBytes(haystack);
        return Find(haystackBytes, offset);
    }

    /// <summary>
    ///   Find the extent of the first match.
    /// </summary>
    /// <param name="haystack">The string to search for this pattern</param>
    public Match Find(string haystack) => Find(haystack, 0);

    /// <summary>
    ///   Find All Matches
    ///   <para>Returns an iterator over each match in the pattern</para>
    /// </summary>
    /// <param name="haystack">The string to search for this pattern</param>
    public IEnumerable<Match> FindAll(byte[] haystack)
    {
        using MatchIter iter = new(this, haystack);
        while (iter.MoveNext())
        {
            yield return iter.Current;
        }
    }

    /// <summary>
    ///   Find All Matches
    ///   <para>Returns an iterator over each match in the pattern</para>
    /// </summary>
    /// <param name="haystack">The string to search for this pattern</param>
    public IEnumerable<Match> FindAll(string haystack)
    {
        byte[] haystackBytes = Encoding.UTF8.GetBytes(haystack);
        return FindAll(haystackBytes);
    }

    /// <summary>
    ///   Get Capture Index from Name
    /// </summary>
    public int this[string capture] =>
        RureFfi.rure_capture_name_index(Raw, Encoding.UTF8.GetBytes(capture));

    /// <summary>
    ///   Get an enumeration of all the named captures in this pattern.
    /// </summary>
    public IEnumerable<string> CaptureNames()
    {
        return new CaptureNamesEnumerable(this);
    }

    /// <summary>
    ///   Captures - Find the extent of the capturing groups in the pattern
    ///   in a given haystack.
    /// </summary>
    /// <param name="haystack">The string to search for this pattern</param>
    /// <param name="offset">The offset to start searching at</param>
    public Captures Captures(byte[] haystack, uint offset)
    {
        Captures caps = new(this, haystack);
        bool matched = RureFfi.rure_find_captures(Raw,
                                                 haystack,
                                                 new UIntPtr((uint)haystack.Length),
                                                 new UIntPtr(offset),
                                                 caps.Raw);

        caps.Matched = matched;
        return caps;
    }

    /// <summary>
    ///   Captures - Find the extent of the capturing groups in the pattern
    ///   in a given haystack.
    /// </summary>
    /// <param name="haystack">The string to search for this pattern</param>
    public Captures Captures(byte[] haystack) => Captures(haystack, 0);

    /// <summary>
    ///   Captures - Find the extent of the capturing groups in the pattern
    ///   in a given haystack.
    /// </summary>
    /// <param name="haystack">The string to search for this pattern</param>
    /// <param name="offset">The offset to start searching at</param>
    public Captures Captures(string haystack, uint offset)
    {
        byte[] haystackBytes = Encoding.UTF8.GetBytes(haystack);
        return Captures(haystackBytes, offset);
    }

    /// <summary>
    ///   Captures - Find the extent of the capturing groups in the pattern
    ///   in a given haystack.
    /// </summary>
    /// <param name="haystack">The string to search for this pattern</param>
    public Captures Captures(string haystack) => Captures(haystack, 0);

    /// <summary>
    ///   Capture All Matches
    ///   <para>
    ///     Returns an iterator containing the capture information
    ///     for each match of the pattern.
    ///   </para>
    /// </summary>
    /// <param name="haystack">The string to search for this pattern</param>
    public IEnumerable<Captures> CaptureAll(byte[] haystack)
    {
        using CapturesIter iter = new(this, haystack);
        while (iter.MoveNext())
        {
            yield return iter.Current;
        }
    }

    /// <summary>
    ///   Capture All Matches
    ///   <para>
    ///     Returns an iterator containing the capture information
    ///     for each match of the pattern.
    ///   </para>
    /// </summary>
    /// <param name="haystack">The string to search for this pattern</param>
    public IEnumerable<Captures> CaptureAll(string haystack)
    {
        byte[] haystackBytes = Encoding.UTF8.GetBytes(haystack);
        return CaptureAll(haystackBytes);
    }

    /// <summary>
    ///   Releases the unmanaged resources used by the Regex and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">
    ///   true to release both managed and unmanaged resources; false to release only unmanaged resources.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            // Dispose managed resources
        }

        // Dispose unmanaged resources
        Raw.Dispose();
        _disposed = true;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///   The default flags for the regex
    /// </summary>
    public static RureFlags DefaultFlags => RureFlags.Unicode;

    /// <summary>
    ///   Finalizer
    /// </summary>
    ~Regex()
    {
        Dispose(false);
    }
}
