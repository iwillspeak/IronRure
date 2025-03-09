using System;
using Xunit;

namespace IronRure.Tests;

public class ErrorTests
{
    [Fact]
    public void Error_Create_Succeeds()
    {
        ErrorHandle err = RureFfi.rure_error_new();
        Assert.NotNull(err);
    }

    [Fact]
    public void Error_AsIDisposable_ImplementsInterface()
    {
        ErrorHandle err = RureFfi.rure_error_new();
        IDisposable dispo = err as IDisposable;

        Assert.NotNull(dispo);
    }
}
