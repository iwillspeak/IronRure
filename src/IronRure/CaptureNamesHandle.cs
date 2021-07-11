using System;
using System.Runtime.InteropServices;

namespace IronRure
{
    /// <summary>Pointer to a capture names iterator</summary>
    public sealed class CaptureNamesHandle : SafeHandle
    {
        /// <summary>Initialise a capture names handle with the default value</summary>
        public CaptureNamesHandle()
            : base(IntPtr.Zero, true)
        {
        }

        /// <inheritdoc />
        public override bool IsInvalid => handle == IntPtr.Zero;

        /// <inheritdoc />
        protected override bool ReleaseHandle()
        {
            RureFfi.rure_iter_capture_names_free(handle);
            return true;
        }
    }
}
