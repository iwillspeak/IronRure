using System;
using Xunit;

using IronRure;

namespace IronRureTests
{
    public class OptionsTets
    {
        [Fact]
        public void Options_Create_Succeeds()
        {
            var opts = new Options();
        }

        [Fact]
        public void Options_AsIDisposable_ImplementsInterface()
        {
            var opts = new Options();
            var dispo = opts as IDisposable;

            Assert.NotNull(dispo);
        }

        [Fact]
        public void Options_GetRaw_ReturnsValidUntilDisposed()
        {
            var opts = new Options();

            Assert.NotEqual(IntPtr.Zero, opts.Raw);

            opts.Dispose();
            Assert.Equal(IntPtr.Zero, opts.Raw);
        }

        [Fact]
        public void Options_SetSizeLimit_Succeeds()
        {
            using (var opts = new Options())
            {
                opts.Size = 1024;
            }
        }

        [Fact]
        public void Options_SetDfaLimit_Succeeds()
        {
            using (var opts = new Options())
            {
                opts.DfaSize = 1024;
            }
        }

        [Fact]
        public void Options_WithBuildableInterface_Succeeds()
        {
            new Options()
                .WithSize(512)
                .WithDfaSize(1024)
                .WithSize(768)
                .Dispose();
        }
    }
}
