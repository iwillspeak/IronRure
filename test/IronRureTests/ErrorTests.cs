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
            var err = RureFfi.rure_error_new();
        }

        [Fact]
        public void Error_AsIDisposable_ImplementsInterface()
        {
            var err = RureFfi.rure_error_new();
            var dispo = err as IDisposable;

            Assert.NotNull(dispo);
        }
    }
}
