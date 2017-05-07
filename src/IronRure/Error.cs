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
    public class Error : IDisposable
    {
        public Error()
        {
            _raw = RureFfi.rure_error_new();
        }

        ~Error() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            var toFree = Interlocked.Exchange(ref _raw, IntPtr.Zero);
            if (toFree != IntPtr.Zero)
                RureFfi.rure_error_free(toFree);
        }

        /// <summary>
        ///   Get the Error Message
        ///   <para>
        ///     This will read the error message from the underlying unmanaged
        ///     objecct and return it.
        ///   </para>
        /// </summary>
        public string Message =>
            Marshal.PtrToStringAnsi(RureFfi.rure_error_message(_raw));

        private IntPtr _raw;
        /// <summary>
        ///   The raw unmanaged pointer this class manages the lifetime of.
        /// </summary>
        public IntPtr Raw => _raw;
    }
}