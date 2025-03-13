using System;

namespace IronRure;

/// <summary>Regex compilation or parse error</summary>
/// <remarks>Initialise the exception with the underlying error message</remarks>
public class RegexCompilationException : Exception
{
    /// <summary>Regex compilation or parse error</summary>
    /// <remarks>Initialise the exception with the underlying error message</remarks>
    public RegexCompilationException(string message) : base($"Error compiling regular expression: {message}")
    {
    }
}
