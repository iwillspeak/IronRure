using System;
using System.Runtime.InteropServices;

namespace IronRure
{
    public sealed class RegexIterHandle : SafeHandle
    {
        public RegexIterHandle()
            : base(IntPtr.Zero, true)
        {
        }

        public override bool IsInvalid => handle == IntPtr.Zero;

        protected override bool ReleaseHandle()
        {
            RureFfi.rure_iter_free(handle);
            return true;
        }
    }
}
