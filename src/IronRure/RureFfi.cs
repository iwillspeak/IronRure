using System;
using System.Runtime.InteropServices;

namespace IronRure
{
    public static class RureFfi
    {
        /// <summary>
        ///   rure_compile compiles the given pattern into a regular expression. The
        ///   pattern must be valid UTF-8 and the length corresponds to the number of
        ///   bytes in the pattern.
        ///  
        ///   flags is a bitfield. Valid values are constants declared with prefix
        ///   RURE_FLAG_.
        ///  
        ///   options contains non-flag configuration settings. If it's NULL, default
        ///   settings are used. options may be freed immediately after a call to
        ///   rure_compile.
        ///  
        ///   error is set if there was a problem compiling the pattern (including if the
        ///   pattern is not valid UTF-8). If error is NULL, then no error information
        ///   is returned. In all cases, if an error occurs, NULL is returned.
        ///  
        ///   The compiled expression returned may be used from multiple threads
        ///   simultaneously.
        /// </summary>
        [DllImport("rure.dll")]
        public static extern IntPtr rure_compile(byte[] pattern, UIntPtr length,
                    uint flags, IntPtr options,
                    IntPtr error);
        
        /// <summary>
        ///   rure_free frees the given compiled regular expression.
        ///  
        ///   This must be called at most once for any rure.
        /// </summary>
        [DllImport("rure.dll")]
        public static extern void rure_free(IntPtr reg);

        /// <summary>
        ///   rure_options_new allocates space for options.
        ///  
        ///   Options may be freed immediately after a call to rure_compile, but otherwise
        ///   may be freely used in multiple calls to rure_compile.
        ///  
        ///   It is not safe to set options from multiple threads simultaneously. It is
        ///   safe to call rure_compile from multiple threads simultaneously using the
        ///   same options pointer.
        /// </summary>
        [DllImport("rure.dll")]
        public static extern IntPtr rure_options_new();

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

        /// <summary>
        ///   rure_error_message returns a NUL terminated string that describes the error
        ///   message.
        ///  
        ///   The pointer returned must not be freed. Instead, it will be freed when
        ///   rure_error_free is called. If err is used in subsequent calls to
        ///   rure_compile, then this pointer may change or become invalid.
        /// </summary>
        [DllImport("rure.dll")]
        public static extern IntPtr rure_error_message(IntPtr err);
    }
}