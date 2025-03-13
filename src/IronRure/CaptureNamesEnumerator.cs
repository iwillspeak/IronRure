using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace IronRure;

internal class CaptureNamesEnumerator : IEnumerator<string>
{
    private readonly CaptureNamesHandle? _handle;
    private bool _disposed;
    private readonly Regex _regex;

    public CaptureNamesEnumerator(Regex regex)
    {
        _regex = regex;
        _handle = regex?.Raw != null ?
            RureFfi.rure_iter_capture_names_new(regex.Raw) : null;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing && _handle != null)
        {
            _handle.Dispose();
            _regex.Dispose();
        }
        _disposed = true;
    }

    // Implement non-nullable Current for IEnumerator<string>
    string IEnumerator<string>.Current => Current ?? string.Empty;

    // Internal implementation can be nullable
    public string? Current { get; private set; }

    object? IEnumerator.Current => Current;

    public bool MoveNext()
    {
        if (_handle == null)
        {
            return false;
        }

        while (RureFfi.rure_iter_capture_names_next(_handle, out var name))
        {
            Current = Marshal.PtrToStringAnsi(name);
            if (!string.IsNullOrEmpty(Current))
            {
                return true;
            }
        }

        Current = null;
        return false;
    }

    public void Reset()
    {
        throw new NotSupportedException();
    }
}
