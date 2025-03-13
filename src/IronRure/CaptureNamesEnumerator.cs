using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace IronRure;

internal class CaptureNamesEnumerator(Regex regex) : IDisposable, IEnumerator<string>
{
    private readonly CaptureNamesHandle _handle = RureFfi.rure_iter_capture_names_new(regex.Raw);

    public void Dispose()
    {
        _handle?.Dispose();
    }

    public string Current { get; protected set; }

    object IEnumerator.Current => Current;


    public bool MoveNext()
    {
        while (RureFfi.rure_iter_capture_names_next(_handle, out var name))
        {
            Current = Marshal.PtrToStringAnsi(name);
            if (!string.IsNullOrEmpty(Current))
            {
                return true;
            }
        }

        return false;
    }

    public void Reset()
    {
        throw new NotSupportedException();
    }
}
