using System;
using System.Runtime.InteropServices;

namespace IronRure
{
    public sealed class CaptureNamesHandle : SafeHandle
    {
        public CaptureNamesHandle()
            : base(IntPtr.Zero, true)
        {
        }

        public override bool IsInvalid => handle == IntPtr.Zero;

        protected override bool ReleaseHandle()
        {
            RureFfi.rure_iter_capture_names_free(handle);
            return true;
        }
    }
}
