using System;
using System.Runtime.InteropServices;

namespace IronRure
{
    /// <summary>
    ///   Rure Match Info Object
    ///   <para>
    ///      This object is defined with the same memory layout as the c struct
    ///      so taht we can pass it to rure_ methods and retrieve match info.
    ///   </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RureMatch
    {
        /// <summary>The UTF-8 code unit offset of the start of this match.</summary>
        public UIntPtr start;

        /// <summary>The UTF-8 code unit offset of the end of this match.</summary>
        public UIntPtr end;
    }
}