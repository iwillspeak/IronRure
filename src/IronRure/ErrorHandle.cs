using System;
using System.Threading;
using System.Runtime.InteropServices;
    
namespace IronRure
{
    /// <summary>
    ///   Managed wrapper around the rure_error object. Ensures that
    ///   error strings get disposed of correctly even when there's
    ///   Exceptions about.
    /// </summary>
    public sealed class ErrorHandle : SafeHandle
    {
        public ErrorHandle()
            : base(IntPtr.Zero, true)
        {
        }

        public override bool IsInvalid => handle == IntPtr.Zero;

        protected override bool ReleaseHandle()
        {
            RureFfi.rure_error_free(handle);
            return true;
        }

        /// <summary>
        ///   Get the Error Message
        ///   <para>
        ///     This will read the error message from the underlying unmanaged
        ///     objecct and return it.
        ///   </para>
        /// </summary>
        public string Message =>
            Marshal.PtrToStringAnsi(RureFfi.rure_error_message(this));

    }
}
