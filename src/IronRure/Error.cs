using System;
using System.Threading;
using System.Runtime.InteropServices;
    
namespace IronRure
{
    /// <summary>
    ///   Managed wrapper around the rure_error object. Ensures that
    ///   error strings get disposed of correctly even when there's
    ///   Exceptions about.
    /// </sumary>
    public class Error : UnmanagedResource
    {
        public Error()
            : base(RureFfi.rure_error_new())
        {}

        protected override void Free(IntPtr resource)
        {
            RureFfi.rure_error_free(resource);
        }

        /// <summary>
        ///   Get the Error Message
        ///   <para>
        ///     This will read the error message from the underlying unmanaged
        ///     objecct and return it.
        ///   </para>
        /// </summary>
        public string Message =>
            Marshal.PtrToStringAnsi(RureFfi.rure_error_message(Raw));
    }
}
