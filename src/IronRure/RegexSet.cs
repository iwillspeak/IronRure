using System;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace IronRure
{
    public class RegexSet : UnmanagedResource
    {
        public RegexSet(params string[] patterns)
            : base(CompileSet(patterns))
        {
            _arity = patterns.Length;
        }

        private static IntPtr CompileSet(string[] patterns)
        {
            var patBytes = patterns.Select(Encoding.UTF8.GetBytes).ToArray();
            var patLengths = patBytes
                .Select(byts => new UIntPtr((uint)byts.Length)).ToArray();
            var patByteHandles = patBytes
                .Select(a => GCHandle.Alloc(a, GCHandleType.Pinned)).ToArray();
            var patBytePinnedPointers = patByteHandles
                .Select(h => h.AddrOfPinnedObject()).ToArray();

            using (var err = new Error())
            {
                var compiled = RureFfi.rure_compile_set(patBytePinnedPointers,
                                                        patLengths,
                                                        new UIntPtr((uint)patLengths.Length),
                                                        (uint)Regex.DefaultFlags,
                                                        IntPtr.Zero,
                                                        err.Raw);

                foreach (var handle in patByteHandles)
                    handle.Free();
                
                if (compiled != IntPtr.Zero)
                    return compiled;

                throw new RegexCompilationException(err.Message);
            }
        }

        /// <summary>
        ///   Is Match - Checks if any of the patterns in the set match.
        /// </summary>
        public bool IsMatch(string haystack)
        {
            var haystackBytes = Encoding.UTF8.GetBytes(haystack);
            return RureFfi.rure_set_is_match(Raw, haystackBytes,
                                             new UIntPtr((uint)haystackBytes.Length),
                                             UIntPtr.Zero);
        }

        /// <summary>
        ///   Matches - Retrieve information about which patterns in the set match.
        /// </summary>
        public SetMatch Matches(string haystack)
        {
            var haystackBytes = Encoding.UTF8.GetBytes(haystack);
            var results = new bool[_arity];
            var overall = RureFfi.rure_set_matches(Raw,
                                                   haystackBytes,
                                                   new UIntPtr((uint)haystackBytes.Length),
                                                   UIntPtr.Zero,
                                                   results);
            return new SetMatch(overall, results);
        }

        protected override void Free(IntPtr resource)
        {
            RureFfi.rure_set_free(resource);
        }

        /// <summary>
        ///   The number of patterns in this set.
        /// </summary>
        private int _arity;
    }
}
