using System;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace IronRure
{
    public class RegexSet : IDisposable
    {
        /// <summary>
        ///   The number of patterns in this set.
        /// </summary>
        private int _arity;

        /// <summary>
        ///  Raw regex set handle
        /// </summary>
        private RegexSetHandle _handle;

        public RegexSet(params string[] patterns)
            : this(Regex.DefaultFlags, patterns)
        {}

        public RegexSet(RureFlags flags, params string[] patterns)
            : this(CompileSet(patterns, flags, new OptionsHandle()), patterns.Length)
        {
        }

        public RegexSet(RureFlags flags, Options options, params string[] patterns)
            : this(CompileSet(patterns, flags, options.Raw), patterns.Length)
        {
        }

        private RegexSet(RegexSetHandle handle, int arity)
        {
            _arity = arity;
            _handle = handle;
        }

        private static RegexSetHandle CompileSet(string[] patterns, RureFlags flags, OptionsHandle options)
        {
            var patBytes = patterns.Select(Encoding.UTF8.GetBytes).ToArray();
            var patLengths = patBytes
                .Select(byts => new UIntPtr((uint)byts.Length)).ToArray();
            var patByteHandles = patBytes
                .Select(a => GCHandle.Alloc(a, GCHandleType.Pinned)).ToArray();
            var patBytePinnedPointers = patByteHandles
                .Select(h => h.AddrOfPinnedObject()).ToArray();

            using (var err = RureFfi.rure_error_new())
            {
                var compiled = RureFfi.rure_compile_set(patBytePinnedPointers,
                                                        patLengths,
                                                        new UIntPtr((uint)patLengths.Length),
                                                        (uint)flags,
                                                        options,
                                                        err);

                foreach (var handle in patByteHandles)
                    handle.Free();
                
                if (compiled.IsInvalid)
                    throw new RegexCompilationException(err.Message);

                return compiled;
            }
        }

        /// <summary>
        ///   Is Match - Checks if any of the patterns in the set match.
        /// </summary>
        public bool IsMatch(string haystack) =>
            IsMatch(Encoding.UTF8.GetBytes(haystack));

        /// <summary>
        ///   Is match - Check if any of the patterns in the set match.
        /// </summary>
        public bool IsMatch(byte[] haystack)
        {
            return RureFfi.rure_set_is_match(_handle, haystack,
                                             new UIntPtr((uint)haystack.Length),
                                             UIntPtr.Zero);
        }

        /// <summary>
        ///   Matches - Retrieve information about which patterns in the set match.
        /// </summary>
        public SetMatch Matches(string haystack) =>
            Matches(Encoding.UTF8.GetBytes(haystack));

        /// <summary>
        ///   Matches - Retrieve information abut which patterns in the set match.
        /// </summary>
        public SetMatch Matches(byte[] haystack)
        {
            var results = new bool[_arity];
            var overall = RureFfi.rure_set_matches(_handle,
                                                   haystack,
                                                   new UIntPtr((uint)haystack.Length),
                                                   UIntPtr.Zero,
                                                   results);
            return new SetMatch(overall, results);
        }

        public void Dispose()
        {
            _handle.Dispose();
        }
    }
}
