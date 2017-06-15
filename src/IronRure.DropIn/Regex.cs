using System;
using SysRegex = System.Text.RegularExpressions.Regex;

/// <summary>
///   IronRure DropIn - System.Text.RegularExpressions compatible API
///   to the IronRure package.
/// </summary>
namespace IronRure.DropIn
{
    /// <summary>
    ///   Regular Expression Wrapper
    ///   <para>
    ///     This class represents a wrapper around an underlying
    ///     IronRure <see cref="IronRure.Regex" />. It exposes an API
    ///     similar to that of a standard <see
    ///     cref="Sytem.Text.RegularExpressions.Regex" />
    /// </summary>
    public class Regex
    {
        private readonly IronRure.Regex _pattern;

        /// <summary>
        ///   Create a Regex which matches the given pattern
        /// </summary>
        public Regex(string pattern)
            : this(pattern, RegexOptions.None)
        {}

        /// <summary>
        ///   Create a Regex from a given pattern and options.
        /// </summary>
        public Regex(string pattern, RegexOptions options)
            : this(pattern, options, TimeSpan.Zero)
        {}

        /// <summary>
        ///   Create a Regex from a given pattern, ooptions and
        ///   timeout.
        ///   <para>
        ///     Note: the timeout is ignored. Rure does not support
        ///     this functionality as it has guaranteed bounds on
        ///     execution time instead.
        ///   </para>
        /// </summary>
        public Regex(string pattern, RegexOptions options, TimeSpan timeout)
        {
            Options = options;
            var rureOptions = new IronRure.Options();
            var rureFlags = default(IronRure.RureFlags);
            _pattern = new IronRure.Regex(pattern, rureOptions, rureFlags);
            MatchTimeout = timeout;
        }

        /// <summary>
        ///   Escape any special characters in the pattern so that
        ///   they will be interpreted literally when used as part of
        ///   a regex match.
        /// </summary>
        public static string Escape(string pattern) => SysRegex.Escape(pattern);

        /// <summary>
        ///   Get the timout that was passed in when this regex was
        ///   created.
        /// </summary>
        public TimeSpan MatchTimeout {get;}

        /// <summary>
        ///   Get the options used when this regex was created.
        /// </summary>
        public RegexOptions Options {get;}

        /// <summary>
        ///   Is this regex Right-to-Left?
        /// </summary>
        public bool RightToLeft => Options.HasFlag(RegexOptions.RightToLeft);
    }
}
