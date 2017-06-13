using System;

namespace IronRure.DropIn
{
    [Flags]
    public enum RegexOptions
    {
        /// <summary>
        ///   Specified that the regular expression should be compiled
        ///   to an assembly. Not supported in IronRure. If used this
        ///   flag will have no effect.
        /// </summary>
        Compiled,

        /// <summary>
        ///   Specifies that the cutlture differences in languages
        ///   should be ignored. Not supported in IronRure. If used
        ///   this flag will have no effect.
        /// </summary>
        CultureInvariant,

        /// <summary>
        ///   Enables ECMAScript-compliant behaviour for the
        ///   expression. Not supported in IronRure. If used this flag
        ///   will have no effect.
        /// </summary>
        ECMAScript,

        /// <summary>
        ///   Specifies that the only valid captures are explicitly
        ///   named or numberd groups. Not supported in IronRure. If
        ///   used this flag will have no effect.
        /// </summary>
        ExplicitCapture,

        /// <summary>
        ///   Specifies case-insensitive matching.
        /// </summary>
        IgnoreCase,

        /// <summary>
        ///   Eliminates unescaped white space from the pattern and
        ///   enables comment markers (`#`).
        /// </summary>
        IgnorePatternWhitespace,

        /// <summary>
        ///   Multiline mode. Changes the meaning of `^` and `$` so
        ///   tthat they match the beginning and end of any line.
        /// </summary>
        Multiline,

        /// <summary>
        ///   Specifies that no options are set.
        /// </summary>
        None,

        /// <summary>
        ///   Specifies that the search will be from right-to-left
        ///   instead of left-to-right.
        /// </summary>
        RightToLeft,

        /// <summary>
        ///   Specifies single-line mode. Changes the meaning of `.`
        ///   (dot) so that it matches every character.
        /// </summary>
        Singleline,
    }
}
