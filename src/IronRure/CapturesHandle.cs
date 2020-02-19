using System;
using System.Runtime.InteropServices;

namespace IronRure
{
    public sealed class CapturesHandle : SafeHandle
    {
        public CapturesHandle()
            : base(IntPtr.Zero, true)
        {
        }

        public override bool IsInvalid => handle == IntPtr.Zero;

        protected override bool ReleaseHandle()
        {
            RureFfi.rure_captures_free(handle);
            return true;
        }
    }
}
