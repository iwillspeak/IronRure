using System;
using System.Runtime.InteropServices;

namespace IronRure
{
    /// <summary>Raw handle to a captures info object</summary>
    public sealed class CapturesHandle : SafeHandle
    {
        /// <summary>Initialise a captures handle with the default value</summary>
        public CapturesHandle()
            : base(IntPtr.Zero, true)
        {
        }

        /// <inheritdoc />
        public override bool IsInvalid => handle == IntPtr.Zero;

        /// <inheritdoc />
        protected override bool ReleaseHandle()
        {
            RureFfi.rure_captures_free(handle);
            return true;
        }
    }
}
