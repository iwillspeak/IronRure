using System;
using System.Runtime.InteropServices;

namespace IronRure;

/// <summary>Pointer to a regex match iterator</summary>
public sealed class RegexIterHandle : SafeHandle
{
    /// <summary>Create a new regex match iterator with the default value</summary>
    public RegexIterHandle()
        : base(IntPtr.Zero, true)
    {
    }

    /// <inheritdoc />
    public override bool IsInvalid => handle == IntPtr.Zero;

    /// <inheritdoc />
    protected override bool ReleaseHandle()
    {
        RureFfi.rure_iter_free(handle);
        return true;
    }
}
