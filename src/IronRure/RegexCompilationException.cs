namespace IronRure
{
    using System;

    public class RegexCompilationException : Exception
    {
        public RegexCompilationException(string message)
            : base($"Error compiling regular expression: {message}")
        {
        }
    }
}