namespace IronRure;

/// <summary>Builder API for <see cref="Options" />.</summary>
public static class OptionsBuilder
{
    /// <summary>Set the size for the compiled expression.</summary>
    /// <param name="opts">The options to update.</param>
    /// <param name="size">The size limit.</param>
    public static Options WithSize(this Options opts, uint size)
    {
        opts.Size = size;
        return opts;
    }

    /// <summary>Set the DFA size for the compiled expression.</summary>
    /// <param name="opts">The options to update.</param>
    /// <param name="size">The DFA size limit.</param>
    public static Options WithDfaSize(this Options opts, uint size)
    {
        opts.DfaSize = size;
        return opts;
    }
}
