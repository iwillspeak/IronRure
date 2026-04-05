using Xunit;

namespace IronRure.Tests;

public class OptionsTests
{
    [Fact]
    public void Options_Create_Succeeds()
    {
        Options opts = new();
        Assert.NotNull(opts);
    }

    [Fact]
    public void Options_AsIDisposable_ImplementsInterface()
    {
        Options opts = new();
        opts.Dispose();

        Assert.NotNull(opts);
    }

    [Fact]
    public void Options_SetSizeLimit_Succeeds()
    {
        using Options opts = new();
        opts.Size = 1024;
    }

    [Fact]
    public void Options_SetDfaLimit_Succeeds()
    {
        using Options opts = new();
        opts.DfaSize = 1024;
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
