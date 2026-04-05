using System;
using System.Runtime.InteropServices;

namespace IronRure;

/// <summary>Raw pointer to a compiled regular expression.</summary>
public sealed class RegexHandle : SafeHandle
{
    /// <summary>Initialise a new regex handle with the default value.</summary>
    public RegexHandle()
        : base(IntPtr.Zero, true)
    {
    }

    /// <inheritdoc />
    public override bool IsInvalid => handle == IntPtr.Zero;

    /// <inheritdoc />
    protected override bool ReleaseHandle()
    {
        RureFfi.rure_free(handle);
        return true;
    }
}
