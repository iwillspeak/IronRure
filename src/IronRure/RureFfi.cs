using System;
using System.Runtime.InteropServices;

namespace IronRure
{
    public static class RureFfi
    {
        /// <summary>
        /// rure_error_new allocates space for an error.
        /// 
        /// If error information is desired, then rure_error_new should be called
        /// to create an rure_error pointer, and that pointer can be passed to
        /// rure_compile. If an error occurred, then rure_compile will return NULL and
        /// the error pointer will be set. A message can then be extracted.
        /// 
        /// It is not safe to use errors from multiple threads simultaneously. An error
        /// value may be reused on subsequent calls to rure_compile.
        /// </summary>
        [DllImport("rure.dll")]
        public static extern IntPtr rure_error_new();

        /// <summary>
        /// rure_error_free frees the error given.
        ///
        /// This must be called at most once.
        /// </summary>
        [DllImport("rure.dll")]
        public static extern void rure_error_free(IntPtr error);
    }
}