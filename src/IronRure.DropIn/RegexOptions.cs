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
        Compiled = 0x0001,

        /// <summary>
        ///   Specifies that the cutlture differences in languages
        ///   should be ignored. Not supported in IronRure. If used
        ///   this flag will have no effect.
        /// </summary>
        CultureInvariant = 0x0002,

        /// <summary>
        ///   Enables ECMAScript-compliant behaviour for the
        ///   expression. Not supported in IronRure. If used this flag
        ///   will have no effect.
        /// </summary>
        ECMAScript = 0x0004,

        /// <summary>
        ///   Specifies that the only valid captures are explicitly
        ///   named or numberd groups. Not supported in IronRure. If
        ///   used this flag will have no effect.
        /// </summary>
        ExplicitCapture = 0x0008,

        /// <summary>
        ///   Specifies case-insensitive matching.
        /// </summary>
        IgnoreCase = 0x0010,

        /// <summary>
        ///   Eliminates unescaped white space from the pattern and
        ///   enables comment markers (`#`).
        /// </summary>
        IgnorePatternWhitespace = 0x0020,

        /// <summary>
        ///   Multiline mode. Changes the meaning of `^` and `$` so
        ///   tthat they match the beginning and end of any line.
        /// </summary>
        Multiline = 0x0040,

        /// <summary>
        ///   Specifies that no options are set.
        /// </summary>
        None = 0x0080,

        /// <summary>
        ///   Specifies that the search will be from right-to-left
        ///   instead of left-to-right.
        /// </summary>
        RightToLeft = 0x0100,

        /// <summary>
        ///   Specifies single-line mode. Changes the meaning of `.`
        ///   (dot) so that it matches every character.
        /// </summary>
        Singleline = 0x0200,
    }
}
