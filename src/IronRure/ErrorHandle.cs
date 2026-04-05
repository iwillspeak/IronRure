using System;
using System.Runtime.InteropServices;

namespace IronRure;

/// <summary>
///     Managed wrapper around the rure_error object. Ensures that
///     error strings get disposed of correctly even when there's
///     Exceptions about.
/// </summary>
public sealed class ErrorHandle : SafeHandle
{
    /// <summary>Initialise a new error handle with the default value</summary>
    public ErrorHandle()
        : base(IntPtr.Zero, true)
    {
    }

    /// <inheritdoc />
    public override bool IsInvalid => handle == IntPtr.Zero;

    /// <summary>
    ///     Get the Error Message
    ///     <para>
    ///         This will read the error message from the underlying unmanaged
    ///         objecct and return it.
    ///     </para>
    /// </summary>
    public string Message
    {
        get
        {
            var messagePtr = RureFfi.rure_error_message(this);
            return messagePtr == IntPtr.Zero ? null : Marshal.PtrToStringAnsi(messagePtr);
        }
    }

    /// <inheritdoc />
    protected override bool ReleaseHandle()
    {
        RureFfi.rure_error_free(handle);
        return true;
    }
}
