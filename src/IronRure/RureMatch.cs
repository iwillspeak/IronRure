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
        public UIntPtr start;
        public UIntPtr end;
    }
}