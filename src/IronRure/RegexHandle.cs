using System;
using System.Runtime.InteropServices;

namespace IronRure
{
    public sealed class RegexHandle : SafeHandle
    {
        public RegexHandle()
            : base(IntPtr.Zero, true)
        {
        }

        public override bool IsInvalid => handle == IntPtr.Zero;

        protected override bool ReleaseHandle()
        {
            RureFfi.rure_free(handle);
            return true;
        }
    }
}
