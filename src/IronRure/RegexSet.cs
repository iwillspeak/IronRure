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
        {}

        private static IntPtr CompileSet(string[] patterns)
        {
            var patBytes = patterns.Select(Encoding.UTF8.GetBytes).ToArray();
            var patLengths = patBytes.Select(byts => new UIntPtr((uint)byts.Length)).ToArray();
            var patBytePointers = patBytes.Select(GCHandle.Alloc).ToArray();
            var patBytePinnedPointers = patBytePointers.Select(h => GCHandle.ToIntPtr(h)).ToArray();

            using (var err = new Error())
            {
                var compiled = RureFfi.rure_compile_set(patBytePinnedPointers,
                                                        patLengths,
                                                        new UIntPtr((uint)patLengths.Length),
                                                        0U,
                                                        IntPtr.Zero,
                                                        err.Raw);
                
                if (compiled != IntPtr.Zero)
                    return compiled;

                throw new RegexCompilationException(err.Message);
            }
        }

        protected override void Free(IntPtr resource)
        {
            RureFfi.rure_set_free(resource);
        }
    }
}