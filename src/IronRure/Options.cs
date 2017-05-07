using System;
using System.Threading;

namespace IronRure
{
    public class Options : IDisposable
    {
        public Options()
        {
            _raw = RureFfi.rure_options_new();
        }

        ~Options() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            var toFree = Interlocked.Exchange(ref _raw, IntPtr.Zero);
            if (toFree != IntPtr.Zero)
                RureFfi.rure_options_free(toFree);
        }

        /// <summary>
        ///   Set Size Limit - Controls the size of a single regex program
        /// </summary>
        public uint Size
        {
            set => RureFfi.rure_options_size_limit(_raw, new UIntPtr(value));
        }

        /// <summary>
        ///   Set DFA Size Limit - Controls the DFA cache size during search
        /// </summary>
        public uint DfaSize
        {
            set => RureFfi.rure_options_dfa_size_limit(_raw, new UIntPtr(value));
        }

        /// <summary>
        ///   Raw rure_options refernece
        /// </summary>
        public IntPtr Raw => _raw;
        private IntPtr _raw;
    }
}
