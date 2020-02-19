using System;

namespace IronRure
{
    public class Options : IDisposable
    {
        public Options()
        {
            Raw = RureFfi.rure_options_new();
        }

        internal OptionsHandle Raw { get; }

        /// <summary>
        ///   Set Size Limit - Controls the size of a single regex program
        /// </summary>
        public uint Size
        {
            set => RureFfi.rure_options_size_limit(Raw, new UIntPtr(value));
        }

        /// <summary>
        ///   Set DFA Size Limit - Controls the DFA cache size during search
        /// </summary>
        public uint DfaSize
        {
            set => RureFfi.rure_options_dfa_size_limit(Raw, new UIntPtr(value));
        }

        public void Dispose()
        {
            Raw.Dispose();
        }
    }
}
