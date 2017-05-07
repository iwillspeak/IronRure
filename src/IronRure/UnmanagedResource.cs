using System;
using System.Threading;

namespace IronRure
{
    public abstract class UnmanagedResource : IDisposable
    {
        protected UnmanagedResource(IntPtr raw)
        {
            _raw = raw;
        }

        ~UnmanagedResource() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            var toFree = Interlocked.Exchange(ref _raw, IntPtr.Zero);
            if (toFree != IntPtr.Zero)
                Free(toFree);
        }

        protected abstract void Free(IntPtr resource);

        /// <summary>
        ///   The raw unmanaged pointer this class manages the lifetime of.
        /// </summary>
        public IntPtr Raw => _raw;
        private IntPtr _raw;
    }
}
