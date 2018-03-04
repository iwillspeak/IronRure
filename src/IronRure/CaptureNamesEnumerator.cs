using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace IronRure
{
    internal class CaptureNamesEnumerator : UnmanagedResource, IEnumerator<string>
    {
        public string Current { get; protected set; }

        object IEnumerator.Current => (object)Current;

        public CaptureNamesEnumerator(Regex regex)
            : base(RureFfi.rure_iter_capture_names_new(regex.Raw))
        {}

        protected override void Free(IntPtr resource)
        {
            RureFfi.rure_iter_capture_names_free(resource);
        }

        public bool MoveNext()
        {
            while (RureFfi.rure_iter_capture_names_next(Raw, out IntPtr name))
            {
                Current = Marshal.PtrToStringAnsi(name);
                if (!string.IsNullOrEmpty(Current))
                    return true;
            }
            return false;
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }
    }
}
