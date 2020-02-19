using System;
using System.Runtime.InteropServices;

namespace IronRure
{
    public sealed class RegexSetHandle : SafeHandle
    {
        public RegexSetHandle()
            : base(IntPtr.Zero, true)
        {
        }

        public override bool IsInvalid => handle == IntPtr.Zero;

        protected override bool ReleaseHandle()
        {
            RureFfi.rure_set_free(handle);
            return true;
        }
    }
}
