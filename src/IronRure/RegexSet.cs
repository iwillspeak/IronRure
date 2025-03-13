using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace IronRure;

/// <summary>
///     Regex set. Used to match a collection of patterns against input text
///     at the same time.
/// </summary>
public class RegexSet : IDisposable
{
    /// <summary>
    ///     The number of patterns in this set.
    /// </summary>
    private readonly int _arity;

    /// <summary>
    ///     Raw regex set handle
    /// </summary>
    private RegexSetHandle? _handle;

    /// <summary>
    ///     Create a regex set from a given collection of patterns with the
    ///     default flags and options.
    /// </summary>
    /// <param name="patterns">The patterns for this set.</param>
    public RegexSet(params string[] patterns)
        : this(Regex.DefaultFlags, patterns)
    {
    }

    /// <summary>
    ///     Create a regex set from a given collection of patterns with the
    ///     given <paramref name="flags" /> and default options.
    /// </summary>
    /// <param name="flags">The flags to use for this set.</param>
    /// <param name="patterns">The patterns for this set.</param>
    public RegexSet(RureFlags flags, params string[] patterns)
        : this(CompileSet(patterns, flags, new OptionsHandle()), patterns.Length)
    {
    }

    /// <summary>
    ///     Create a regex set from a given collection of patterns with the
    ///     given <paramref name="flags" /> and <paramref name="options" />.
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

    /// <summary>
    ///     Disposes the resources used by the RegexSet.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private static RegexSetHandle CompileSet(string[] patterns, RureFlags flags, OptionsHandle options)
    {
        var patBytes = patterns.Select(Encoding.UTF8.GetBytes).ToArray();
        var patLengths = patBytes
            .Select(bytes => new UIntPtr((uint)bytes.Length)).ToArray();
        var patByteHandles = new GCHandle[patBytes.Length];

        try
        {
            // Allocate handles and pin memory
            for (var i = 0; i < patBytes.Length; i++)
            {
                patByteHandles[i] = GCHandle.Alloc(patBytes[i], GCHandleType.Pinned);
            }

            var patBytePinnedPointers = patByteHandles
                .Select(h => h.AddrOfPinnedObject()).ToArray();

            using var err = RureFfi.rure_error_new();
            var compiled = RureFfi.rure_compile_set(patBytePinnedPointers,
                patLengths,
                new UIntPtr((uint)patLengths.Length),
                (uint)flags,
                options,
                err);

            if (compiled.IsInvalid)
            {
                throw new RegexCompilationException(err.Message);
            }

            return compiled;
        }
        finally
        {
            // Always free handles even if an exception occurs
            foreach (var handle in patByteHandles)
            {
                if (handle.IsAllocated)
                {
                    handle.Free();
                }
            }
        }
    }

    /// <summary>
    ///     Is Match - Checks if any of the patterns in the set match.
    /// </summary>
    /// <param name="haystack">The string to match against.</param>
    /// <returns>True if any pattern matches, false otherwise.</returns>
    public bool IsMatch(string haystack)
    {
        return IsMatch(Encoding.UTF8.GetBytes(haystack));
    }

    /// <summary>
    ///     Is match - Check if any of the patterns in the set match.
    /// </summary>
    /// <param name="haystack">The byte array to match against.</param>
    /// <returns>True if any pattern matches, false otherwise.</returns>
    public bool IsMatch(byte[] haystack)
    {
        ObjectDisposedException.ThrowIf(_handle?.IsInvalid ?? true, nameof(RegexSet));

        return RureFfi.rure_set_is_match(_handle, haystack,
            new UIntPtr((uint)haystack.Length),
            UIntPtr.Zero);
    }

    /// <summary>
    ///     Matches - Retrieve information about which patterns in the set match.
    /// </summary>
    /// <param name="haystack">The string to match against.</param>
    /// <returns>A SetMatch object containing match information.</returns>
    public SetMatch Matches(string haystack)
    {
        return Matches(Encoding.UTF8.GetBytes(haystack));
    }

    /// <summary>
    ///     Matches - Retrieve information about which patterns in the set match.
    /// </summary>
    /// <param name="haystack">The byte array to match against.</param>
    /// <returns>A SetMatch object containing match information.</returns>
    public SetMatch Matches(byte[] haystack)
    {
        ObjectDisposedException.ThrowIf(_handle?.IsInvalid ?? true, nameof(RegexSet));

        var results = new bool[_arity];
        var overall = RureFfi.rure_set_matches(_handle,
            haystack,
            new UIntPtr((uint)haystack.Length),
            UIntPtr.Zero,
            results);
        return new SetMatch(overall, results);
    }

    /// <summary>
    ///     Disposes the resources used by the RegexSet.
    /// </summary>
    /// <param name="disposing">Indicates if managed resources should be disposed.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposing || _handle is not { IsInvalid: false })
        {
            return;
        }

        _handle.Dispose();
        _handle = null;
    }
}
