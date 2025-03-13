using System;
using Xunit;

namespace IronRure.Tests;

public class ErrorTests
{
    [Fact]
    public void Error_Create_Succeeds()
    {
        var err = RureFfi.rure_error_new();
        Assert.NotNull(err);
    }

    [Fact]
    public void Error_AsIDisposable_ImplementsInterface()
    {
        var err = RureFfi.rure_error_new();
        var dispo = err as IDisposable;

        Assert.NotNull(dispo);
    }
}
