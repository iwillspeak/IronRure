using System;

namespace IronRure;

/// <summary>
/// Regex compilation options.
/// </summary>
public sealed class Options : IDisposable
{
    private bool _disposed;

    /// <summary>
    /// Creates a new options instance with the default values.
    /// </summary>
    public Options()
    {
        Raw = RureFfi.rure_options_new();
    }

    internal OptionsHandle Raw { get; }

    /// <summary>
    /// Sets the size limit for a single regex program.
    /// </summary>
    public uint Size
    {
        set
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Options));
            RureFfi.rure_options_size_limit(Raw, new UIntPtr(value));
        }
    }

    /// <summary>
    /// Sets the DFA size limit for the DFA cache during search.
    /// </summary>
    public uint DfaSize
    {
        set
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Options));
            RureFfi.rure_options_dfa_size_limit(Raw, new UIntPtr(value));
        }
    }

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
}
