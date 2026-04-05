using System;
using System.Runtime.InteropServices;

namespace IronRure;

/// <summary>Raw pointer to a regex options structure.</summary>
public sealed class OptionsHandle : SafeHandle
{
    /// <summary>Initialise an options pointer with the default value.</summary>
    public OptionsHandle()
        : base(IntPtr.Zero, true)
    {
    }

    /// <inheritdoc />
    public override bool IsInvalid => handle == IntPtr.Zero;

    /// <inheritdoc />
    protected override bool ReleaseHandle()
    {
        RureFfi.rure_options_free(handle);
        return true;
    }
}
