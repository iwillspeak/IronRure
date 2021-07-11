namespace IronRure
{
    using System;

    /// <summary>Regex compilation or parse error</summary> 
    public class RegexCompilationException : Exception
    {
        /// <summary>Initialise the exception with the underlying error message</summary>
        public RegexCompilationException(string message)
            : base($"Error compiling regular expression: {message}")
        {
        }
    }
}