using System;
using System.Runtime.InteropServices;

namespace IronRure
{
    public sealed class OptionsHandle : SafeHandle
    {
        public OptionsHandle()
            : base(IntPtr.Zero, true)
        {
        }

        public override bool IsInvalid => handle == IntPtr.Zero;

        protected override bool ReleaseHandle()
        {
            RureFfi.rure_options_free(handle);
            return true;
        }
    }
}
