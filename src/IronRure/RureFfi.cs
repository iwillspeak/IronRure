using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;

namespace IronRure
{
    public static class RureFfi
    {
#if NET45
        private static bool TryLoadFrom(string basePath)
        {
            var location = Path.Combine(
                basePath,
                Environment.Is64BitProcess ? "rure_x64" : "rure_x86",
                "rure.dll");

            var hmod = LoadLibrary(location);
            return hmod != IntPtr.Zero;
        }

        static RureFfi()
        {
            var currentLocation = new Uri(typeof(RureFfi).Assembly.CodeBase).LocalPath;
            if (!TryLoadFrom(Path.GetDirectoryName(currentLocation)))
            {
                TryLoadFrom(AppDomain.CurrentDomain.BaseDirectory);
            }
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string dllToLoad);
#endif

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
        [DllImport("rure", CallingConvention=CallingConvention.Cdecl)]
        public static extern RegexHandle rure_compile(byte[] pattern, UIntPtr length,
                    uint flags, OptionsHandle options,
                    ErrorHandle error);
        
        /// <summary>
        ///   rure_free frees the given compiled regular expression.
        ///  
        ///   This must be called at most once for any rure.
        /// </summary>
        [DllImport("rure", CallingConvention=CallingConvention.Cdecl)]
        public static extern void rure_free(IntPtr reg);

        /// <summary>
        /// rure_is_match returns true if and only if re matches anywhere in haystack.
        ///
        /// haystack may contain arbitrary bytes, but ASCII compatible text is more
        /// useful. UTF-8 is even more useful. Other text encodings aren't supported.
        /// length should be the number of bytes in haystack.
        ///
        /// start is the position at which to start searching. Note that setting the
        /// start position is distinct from incrementing the pointer, since the regex
        /// engine may look at bytes before the start position to determine match
        /// information. For example, if the start position is greater than 0, then the
        /// \A ("begin text") anchor can never match.
        ///
        /// rure_is_match should be preferred to rure_find since it may be faster.
        ///
        /// N.B. The performance of this search is not impacted by the presence of
        /// capturing groups in your regular expression.
        /// </summary>
        [DllImport("rure", CallingConvention=CallingConvention.Cdecl)]
        public static extern bool rure_is_match(RegexHandle re, byte[] haystack, UIntPtr length,
                                                UIntPtr start);
        
        /// <summary>
        ///   rure_find returns true if and only if re matches anywhere in haystack.
        ///   If a match is found, then its start and end offsets (in bytes) are set
        ///   on the match pointer given.
        ///  
        ///   haystack may contain arbitrary bytes, but ASCII compatible text is more
        ///   useful. UTF-8 is even more useful. Other text encodings aren't supported.
        ///   length should be the number of bytes in haystack.
        ///  
        ///   start is the position at which to start searching. Note that setting the
        ///   start position is distinct from incrementing the pointer, since the regex
        ///   engine may look at bytes before the start position to determine match
        ///   information. For example, if the start position is greater than 0, then the
        ///   \A ("begin text") anchor can never match.
        ///  
        ///   rure_find should be preferred to rure_find_captures since it may be faster.
        ///  
        ///   N.B. The performance of this search is not impacted by the presence of
        ///   capturing groups in your regular expression.
        /// </summary>
        [DllImport("rure", CallingConvention=CallingConvention.Cdecl)]
        public static extern bool rure_find(RegexHandle re, byte[] haystack, UIntPtr length,
                                            UIntPtr start, out RureMatch match);

        /// <summary>
        ///   rure_find_captures returns true if and only if re matches anywhere in
        ///   haystack. If a match is found, then all of its capture locations are stored
        ///   in the captures pointer given.
        ///  
        ///   haystack may contain arbitrary bytes, but ASCII compatible text is more
        ///   useful. UTF-8 is even more useful. Other text encodings aren't supported.
        ///   length should be the number of bytes in haystack.
        ///  
        ///   start is the position at which to start searching. Note that setting the
        ///   start position is distinct from incrementing the pointer, since the regex
        ///   engine may look at bytes before the start position to determine match
        ///   information. For example, if the start position is greater than 0, then the
        ///   \A ("begin text") anchor can never match.
        ///  
        ///   Only use this function if you specifically need access to capture locations.
        ///   It is not necessary to use this function just because your regular
        ///   expression contains capturing groups.
        ///  
        ///   Capture locations can be accessed using the rure_captures_* functions.
        ///  
        ///   N.B. The performance of this search can be impacted by the number of
        ///   capturing groups. If you're using this function, it may be beneficial to
        ///   use non-capturing groups (e.g., `(?:re)`) where possible.
        /// </summary>
        [DllImport("rure", CallingConvention=CallingConvention.Cdecl)]
        public static extern bool rure_find_captures(RegexHandle re, byte[] hasytack, UIntPtr length,
                                                     UIntPtr start, CapturesHandle captures);

        /// <summary>
        ///   rure_capture_name_index returns the capture index for the name given. If
        ///   no such named capturing group exists in re, then -1 is returned.
        ///  
        ///   The capture index may be used with rure_captures_at.
        ///  
        ///   This function never returns 0 since the first capture group always
        ///   corresponds to the entire match and is always unnamed.
        /// </summary>
        [DllImport("rure", CallingConvention=CallingConvention.Cdecl)]
        public static extern int rure_capture_name_index(RegexHandle re, byte[] name);

        /// <summary>
        ///   rure_iter_capture_names_new creates a new capture_names
        ///   iterator.
        ///   
        ///   An iterator will report all successive capture group
        ///   names of re.
        /// </summary>
        [DllImport("rure", CallingConvention=CallingConvention.Cdecl)]
        public static extern CaptureNamesHandle rure_iter_capture_names_new(RegexHandle re);
        
        /// <summary>
        ///   rure_iter_capture_names_free frees the iterator given.
        ///  
        ///   It must be called at most once.
        /// </summary>
        [DllImport("rure", CallingConvention=CallingConvention.Cdecl)]
        public static extern void rure_iter_capture_names_free(IntPtr it);

        /// <summary>
        ///   rure_iter_capture_names_next advances the iterator and
        ///   returns true if and only if another capture group name
        ///   exists.
        ///
        ///   The value of the capture group name is written to the
        ///   provided pointer.
        /// </summary>
        [DllImport("rure", CallingConvention=CallingConvention.Cdecl)]
        public static extern bool rure_iter_capture_names_next(CaptureNamesHandle it, out IntPtr name);

        /// <summary>
        ///   rure_iter_new creates a new iterator.
        ///
        ///   An iterator will report all successive non-overlapping
        ///   matches of re.  When calling iterator functions, the
        ///   same haystack and length must be supplied to all
        ///   invocations. (Strict pointer equality is, however, not
        ///   required.)
        /// </summary>
        [DllImport("rure", CallingConvention=CallingConvention.Cdecl)]
        public static extern RegexIterHandle rure_iter_new(RegexHandle re);

        /// <summary>
        ///   rure_iter_free frees the iterator given.
        ///
        ///   It must be called at most once.
        /// </summary>
        [DllImport("rure", CallingConvention=CallingConvention.Cdecl)]
        public static extern void rure_iter_free(IntPtr it);

        /// <summary>
        ///   rure_iter_next advances the iterator and returns true if
        ///   and only if a match was found. If a match is found, then
        ///   the match pointer is set with the start and end location
        ///   of the match, in bytes.
        ///
        ///   If no match is found, then subsequent calls will return
        ///   false indefinitely.
        ///
        ///   haystack may contain arbitrary bytes, but ASCII
        ///   compatible text is more useful. UTF-8 is even more
        ///   useful. Other text encodings aren't supported.  length
        ///   should be the number of bytes in haystack. The given
        ///   haystack must be logically equivalent to all other
        ///   haystacks given to this iterator.
        ///
        ///   rure_iter_next should be preferred to
        ///   rure_iter_next_captures since it may be faster.
        ///
        ///   N.B. The performance of this search is not impacted by
        ///   the presence of capturing groups in your regular
        ///   expression.
        /// </summary>
        [DllImport("rure", CallingConvention=CallingConvention.Cdecl)]
        public static extern bool rure_iter_next(RegexIterHandle it,
                                                 byte[] haystack,
                                                 UIntPtr length,
                                                 out RureMatch match);

        /// <summary>
        ///   rure_iter_next_captures advances the iterator and
        ///   returns true if and only if a match was found. If a
        ///   match is found, then all of its capture locations are
        ///   stored in the captures pointer given.
        ///
        ///   If no match is found, then subsequent calls will return
        ///   false indefinitely.
        ///
        ///   haystack may contain arbitrary bytes, but ASCII
        ///   compatible text is more useful. UTF-8 is even more
        ///   useful. Other text encodings aren't supported.  length
        ///   should be the number of bytes in haystack. The given
        ///   haystack must be logically equivalent to all other
        ///   haystacks given to this iterator.
        /// 
        ///   Only use this function if you specifically need access
        ///   to capture locations.  It is not necessary to use this
        ///   function just because your regular expression contains
        ///   capturing groups.
        /// 
        ///   Capture locations can be accessed using the
        ///   rure_captures_* functions.
        /// 
        ///   N.B. The performance of this search can be impacted by
        ///   the number of capturing groups. If you're using this
        ///   function, it may be beneficial to use non-capturing
        ///   groups (e.g., `(?:re)`) where possible.
        /// </summary>
        [DllImport("rure", CallingConvention=CallingConvention.Cdecl)]
        public static extern bool rure_iter_next_captures(RegexIterHandle it,
                                                          byte[] haystack,
                                                          UIntPtr length,
                                                          CapturesHandle captures);

        /// <summary>
        ///   rure_captures_new allocates storage for all capturing groups in re.
        ///  
        ///   An rure_captures value may be reused on subsequent calls to
        ///   rure_find_captures or rure_iter_next_captures.
        ///  
        ///   An rure_captures value may be freed independently of re, although any
        ///   particular rure_captures should be used only with the re given here.
        ///  
        ///   It is not safe to use an rure_captures value from multiple threads
        ///   simultaneously.
        /// </summary>
        [DllImport("rure", CallingConvention=CallingConvention.Cdecl)]
        public static extern CapturesHandle rure_captures_new(RegexHandle re);

        /// <summary>
        ///   rure_captures_free frees the given captures.
        ///  
        ///   This must be called at most once.
        /// </summary>
        [DllImport("rure", CallingConvention=CallingConvention.Cdecl)]
        public static extern void rure_captures_free(IntPtr captures);

        /// <summary>
        ///   rure_captures_at returns true if and only if the capturing group at the
        ///   index given was part of a match. If so, the given match pointer is populated
        ///   with the start and end location (in bytes) of the capturing group.
        ///  
        ///   If no capture group with the index i exists, then false is
        ///   returned. (A capturing group exists if and only if i is less than
        ///   rure_captures_len(captures).)
        ///  
        ///   Note that index 0 corresponds to the full match.
        /// </summary>
        [DllImport("rure", CallingConvention=CallingConvention.Cdecl)]
        public static extern bool rure_captures_at(CapturesHandle captures,
                                                   UIntPtr i,
                                                   out RureMatch match);

        /// <summary>
        ///   rure_captures_len returns the number of capturing groups in the given
        ///   captures.
        /// </summary>
        [DllImport("rure", CallingConvention=CallingConvention.Cdecl)]
        public static extern UIntPtr rure_captures_len(CapturesHandle captures);

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
        [DllImport("rure", CallingConvention=CallingConvention.Cdecl)]
        public static extern OptionsHandle rure_options_new();
        
        /// <summary>
        ///   rure_options_free frees the given options.
        ///
        ///   This must be called at most once.
        /// </summary>
        [DllImport("rure", CallingConvention=CallingConvention.Cdecl)]
        public static extern void rure_options_free(IntPtr options);

        /// <summary>
        ///   rure_options_size_limit sets the appoximate size limit of the compiled
        ///   regular expression.
        ///  
        ///   This size limit roughly corresponds to the number of bytes occupied by a
        ///   single compiled program. If the program would exceed this number, then a
        ///   compilation error will be returned from rure_compile.
        /// </summary>
        [DllImport("rure", CallingConvention=CallingConvention.Cdecl)]
        public static extern void rure_options_size_limit(OptionsHandle options, UIntPtr limit);

        /// <summary>
        ///   rure_options_dfa_size_limit sets the approximate size of the cache used by
        ///   the DFA during search.
        ///  
        ///   This roughly corresponds to the number of bytes that the DFA will use while
        ///   searching.
        ///  
        ///   Note that this is a *per thread* limit. There is no way to set a global
        ///   limit. In particular, if a regular expression is used from multiple threads
        ///   simultaneously, then each thread may use up to the number of bytes
        ///   specified here.
        /// </summary>
        [DllImport("rure", CallingConvention=CallingConvention.Cdecl)]
        public static extern void rure_options_dfa_size_limit(OptionsHandle options, UIntPtr limit);

        /// <summary>
        ///   rure_compile_set compiles the given list of patterns into a single regular
        ///   expression which can be matched in a linear-scan. Each pattern in patterns
        ///   must be valid UTF-8 and the length of each pattern in patterns corresponds
        ///   to a byte length in patterns_lengths.
        ///  
        ///   The number of patterns to compile is specified by patterns_count. patterns
        ///   must contain at least this many entries.
        ///  
        ///   flags is a bitfield. Valid values are constants declared with prefix
        ///   RURE_FLAG_.
        ///  
        ///   options contains non-flag configuration settings. If it's NULL, default
        ///   settings are used. options may be freed immediately after a call to
        ///   rure_compile.
        ///  
        ///   error is set if there was a problem compiling the pattern.
        ///  
        ///   The compiled expression set returned may be used from multiple threads.
        /// </summary>
        [DllImport("rure", CallingConvention=CallingConvention.Cdecl)]
        public static extern RegexSetHandle rure_compile_set(IntPtr[] patterns,
                                                     UIntPtr[] patterns_lengths,
                                                     UIntPtr patterns_count,
                                                     uint flags,
                                                     OptionsHandle options,
                                                     ErrorHandle error);

        /// <summary>
        ///   rure_set_free frees the given compiled regular expression set.
        ///   
        ///   This must be called at most once for any rure_set.
        /// </summary>
        [DllImport("rure", CallingConvention=CallingConvention.Cdecl)]
        public static extern void rure_set_free(IntPtr re);

        /// <summary>
        ///   rure_is_match returns true if and only if any regexes within the set
        ///   match anywhere in the haystack. Once a match has been located, the
        ///   matching engine will quit immediately.
        ///   
        ///   haystack may contain arbitrary bytes, but ASCII compatible text is more
        ///   useful. UTF-8 is even more useful. Other text encodings aren't supported.
        ///   length should be the number of bytes in haystack.
        ///   
        ///   start is the position at which to start searching. Note that setting the
        ///   start position is distinct from incrementing the pointer, since the regex
        ///   engine may look at bytes before the start position to determine match
        ///   information. For example, if the start position is greater than 0, then the
        ///   \A ("begin text") anchor can never match.
        /// </summary>
        [DllImport("rure", CallingConvention=CallingConvention.Cdecl)]
        public static extern bool rure_set_is_match(RegexSetHandle re,
                                                    byte[] haystack,
                                                    UIntPtr length,
                                                    UIntPtr start);

        /// <summary>
        ///   rure_set_matches compares each regex in the set against the haystack and
        ///   modifies matches with the match result of each pattern. Match results are
        ///   ordered in the same way as the rure_set was compiled. For example,
        ///   index 0 of matches corresponds to the first pattern passed to
        ///   `rure_compile_set`.
        ///   
        ///   haystack may contain arbitrary bytes, but ASCII compatible text is more
        ///   useful. UTF-8 is even more useful. Other text encodings aren't supported.
        ///   length should be the number of bytes in haystack.
        ///   
        ///   start is the position at which to start searching. Note that setting the
        ///   start position is distinct from incrementing the pointer, since the regex
        ///   engine may look at bytes before the start position to determine match
        ///   information. For example, if the start position is greater than 0, then the
        ///   \A ("begin text") anchor can never match.
        ///   
        ///   matches must be greater than or equal to the number of patterns the
        ///   rure_set was compiled with.
        ///   
        ///   Only use this function if you specifically need to know which regexes
        ///   matched within the set. To determine if any of the regexes matched without
        ///   caring which, use rure_set_is_match.
        /// </summary>
        [DllImport("rure", CallingConvention=CallingConvention.Cdecl)]
        public static extern bool rure_set_matches(RegexSetHandle re,
                                                   byte[] haystack,
                                                   UIntPtr length,
                                                   UIntPtr start,
                                                   [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I1)]
                                                   [Out] bool[] matches);

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
        [DllImport("rure", CallingConvention=CallingConvention.Cdecl)]
        public static extern ErrorHandle rure_error_new();

        /// <summary>
        /// rure_error_free frees the error given.
        ///
        /// This must be called at most once.
        /// </summary>
        [DllImport("rure", CallingConvention=CallingConvention.Cdecl)]
        public static extern void rure_error_free(IntPtr error);

        /// <summary>
        ///   rure_error_message returns a NUL terminated string that describes the error
        ///   message.
        ///  
        ///   The pointer returned must not be freed. Instead, it will be freed when
        ///   rure_error_free is called. If err is used in subsequent calls to
        ///   rure_compile, then this pointer may change or become invalid.
        /// </summary>
        [DllImport("rure", CallingConvention=CallingConvention.Cdecl)]
        public static extern IntPtr rure_error_message(ErrorHandle err);
    }
}
