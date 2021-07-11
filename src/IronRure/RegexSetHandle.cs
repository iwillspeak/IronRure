using System;
using System.Runtime.InteropServices;

namespace IronRure
{
    /// <summary>Pointer to a regex set</summary>
    public sealed class RegexSetHandle : SafeHandle
    {
        /// <summary>Initialise a regex set handle with the default value.</summary>
        public RegexSetHandle()
            : base(IntPtr.Zero, true)
        {
        }

        /// <inheritdoc />
        public override bool IsInvalid => handle == IntPtr.Zero;

        /// <inheritdoc />
        protected override bool ReleaseHandle()
        {
            RureFfi.rure_set_free(handle);
            return true;
        }
    }
}
