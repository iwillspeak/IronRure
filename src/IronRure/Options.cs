using System;

namespace IronRure;

/// <summary>Regex compilation options.</summary>
public class Options : IDisposable
{
    private bool _disposed = false;

    /// <summary>Create a new options instance with the default values.</summary>
    public Options()
    {
        Raw = RureFfi.rure_options_new();
    }

    internal OptionsHandle Raw { get; }

    /// <summary>
    ///   Set Size Limit - Controls the size of a single regex program
    /// </summary>
    public uint Size
    {
        set
        {
            ObjectDisposedException.ThrowIf(_disposed, nameof(Options));
            RureFfi.rure_options_size_limit(Raw, new UIntPtr(value));
        }
    }

    /// <summary>
    ///   Set DFA Size Limit - Controls the DFA cache size during search
    /// </summary>
    public uint DfaSize
    {
        set
        {
            ObjectDisposedException.ThrowIf(_disposed, nameof(Options));
            RureFfi.rure_options_dfa_size_limit(Raw, new UIntPtr(value));
        }
    }

    /// <summary>
    ///   Finalizer to ensure resources are released.
    /// </summary>
    ~Options()
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
    ///   Releases the unmanaged resources used by the Options and optionally releases the managed resources.
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
            Raw.Dispose();
        }

        // Dispose unmanaged resources
        _disposed = true;
    }
}
