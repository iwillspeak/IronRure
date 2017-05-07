using System;
using Xunit;

using IronRure;

namespace IronRureTests
{
    public class ErrorTests
    {
        [Fact]
        public void Eror_Create_Succeeds()
        {
            var err = new Error();
        }

        [Fact]
        public void Error_AsIDisposable_ImplementsInterface()
        {
            var err = new Error();
            var dispo = err as IDisposable;

            Assert.NotNull(dispo);
        }

        [Fact]
        public void Error_GetRawHandle_IsValidUntilDisposed()
        {
            var err = new Error();

            Assert.NotEqual(IntPtr.Zero, err.Raw);

            // Should be nulled off after dispose
            err.Dispose();
            Assert.Equal(IntPtr.Zero, err.Raw);
        }
    }
}
