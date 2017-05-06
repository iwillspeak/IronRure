using System;

/// <summary>
///   Iron Rure - .NET Bindings to the Rust Regex Crate
/// </summary>
namespace IronRure
{
    /// <summary>
    ///   Rust Regex
    ///
    ///   <para>
    ///     Managed wrapper around the rust regex class.
    ///   </para>
    /// </summary>
    public class Regex
    {
        public Regex(string pattern)
        {
            var err = RureFfi.rure_error_new();
            RureFfi.rure_error_free(err);
        }
    }
}
