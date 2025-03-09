using System;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace IronRure;

/// <summary>
///   Regex set. Used to match a collection of patterns against input text
///   at the same time.
/// </summary>
public class RegexSet : IDisposable
{
    /// <summary>
    ///   The number of patterns in this set.
    /// </summary>
    private readonly int _arity;

    /// <summary>
    ///  Raw regex set handle
    /// </summary>
    private RegexSetHandle _handle;

    /// <summary>
    ///   Create a regex set from a given collection of patterns with the
    ///   default falgs and options.
    /// </summary>
    /// <param name="patterns">The patterns for this set.</param>
    public RegexSet(params string[] patterns)
        : this(Regex.DefaultFlags, patterns)
    { }

    /// <summary>
    ///   Create a regex set from a given collection of patterns with the
    ///   given <paramref ref="falgs" /> and default options.
    /// </summary>
    /// <param name="flags">The flags to use for this set.</param>
    /// <param name="patterns">The patterns for this set.</param>
    public RegexSet(RureFlags flags, params string[] patterns)
        : this(CompileSet(patterns, flags, new OptionsHandle()), patterns.Length)
    {
    }

    /// <summary>
    ///   Create a regex set from a given collection of patterns with the
    ///   given <paramref ref="falgs" /> and <paramref ref="options" />.
    /// </summary>
    /// <param name="flags">The flags to use for this set.</param>
    /// <param name="options">The options to use for this set.</param>
    /// <param name="patterns">The patterns for this set.</param>
    public RegexSet(RureFlags flags, Options options, params string[] patterns)
        : this(CompileSet(patterns, flags, options.Raw), patterns.Length)
    {
    }

    private RegexSet(RegexSetHandle handle, int arity)
    {
        _arity = arity;
        _handle = handle;
    }

    private static RegexSetHandle CompileSet(string[] patterns, RureFlags flags, OptionsHandle options)
    {
        byte[][] patBytes = [.. patterns.Select(Encoding.UTF8.GetBytes)];
        UIntPtr[] patLengths = [.. patBytes.Select(bytes => new UIntPtr((uint)bytes.Length))];
        var patByteHandles = patBytes
            .Select(a => GCHandle.Alloc(a, GCHandleType.Pinned)).ToArray();
        IntPtr[] patBytePinnedPointers = [.. patByteHandles.Select(h => h.AddrOfPinnedObject())];

        using ErrorHandle err = RureFfi.rure_error_new();
        RegexSetHandle compiled = RureFfi.rure_compile_set(patBytePinnedPointers,
            patLengths,
            new UIntPtr((uint)patLengths.Length),
            (uint)flags,
            options,
            err);

        foreach (GCHandle handle in patByteHandles)
            handle.Free();

        if (compiled.IsInvalid)
            throw new RegexCompilationException(err.Message);

        return compiled;
    }

    /// <summary>
    ///   Is Match - Checks if any of the patterns in the set match.
    /// </summary>
    public bool IsMatch(string haystack) =>
        IsMatch(Encoding.UTF8.GetBytes(haystack));

    /// <summary>
    ///   Is match - Check if any of the patterns in the set match.
    /// </summary>
    public bool IsMatch(byte[] haystack)
    {
        return RureFfi.rure_set_is_match(_handle, haystack,
                                         new UIntPtr((uint)haystack.Length),
                                         UIntPtr.Zero);
    }

    /// <summary>
    ///   Matches - Retrieve information about which patterns in the set match.
    /// </summary>
    public SetMatch Matches(string haystack) =>
        Matches(Encoding.UTF8.GetBytes(haystack));

    /// <summary>
    ///   Matches - Retrieve information abut which patterns in the set match.
    /// </summary>
    public SetMatch Matches(byte[] haystack)
    {
        bool[] results = new bool[_arity];
        bool overall = RureFfi.rure_set_matches(_handle,
                                               haystack,
                                               new UIntPtr((uint)haystack.Length),
                                               UIntPtr.Zero,
                                               results);
        return new SetMatch(overall, results);
    }

    /// <summary>
    ///   Finalizer to ensure resources are released.
    /// </summary>
    ~RegexSet()
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
    ///   Dispose pattern implementation.
    /// </summary>
    /// <param name="disposing">Indicates if managed resources should be disposed.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_handle?.IsInvalid ?? true)
        {
            return;
        }

        _handle?.Dispose();
        _handle = null;
    }
}
