using System;
using System.Collections.Generic;
using System.Text;
using SysRegex = System.Text.RegularExpressions.Regex;

namespace IronRure.DropIn
{
    /// <summary>
    ///   Regular Expression Wrapper
    ///   <para>
    ///     This class represents a wrapper around an underlying
    ///     IronRure <see cref="IronRure.Regex" />. It exposes an API
    ///     similar to that of a standard <see
    ///     cref="System.Text.RegularExpressions.Regex" />.
    ///   </para>
    /// </summary>
    public class Regex
    {
        private readonly IronRure.Regex _pattern;
        private readonly string _patternString;
        private readonly int _captureCount;

        /// <summary>
        ///   Create a Regex which matches the given pattern.
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
        ///   Create a Regex from a given pattern, options and timeout.
        ///   <para>
        ///     Note: the timeout is ignored. Rure does not support
        ///     this functionality as it has guaranteed bounds on
        ///     execution time instead.
        ///   </para>
        /// </summary>
        public Regex(string pattern, RegexOptions options, TimeSpan timeout)
        {
            _patternString = pattern;
            Options = options;
            var rureOptions = new IronRure.Options();
            var rureFlags = RureFlagsFromOptions(options);
            _pattern = new IronRure.Regex(pattern, rureOptions, rureFlags);
            MatchTimeout = timeout;
            // Determine the total number of capture groups (including group 0)
            // once at construction time by creating a temporary Captures handle.
            // rure_captures_new() is a lightweight struct allocation; the result
            // is cached in _captureCount for all subsequent calls.
            using var dummyCaps = _pattern.Captures(Array.Empty<byte>());
            _captureCount = dummyCaps.Length;
        }

        /// <summary>
        ///   Convert a set of RegexOptions into their corresponding
        ///   RureFlags values.
        /// </summary>
        private static RureFlags RureFlagsFromOptions(RegexOptions options)
        {
            var flags = default(RureFlags);
            if (options.HasFlag(RegexOptions.IgnoreCase))
                flags |= RureFlags.Casei;
            if (options.HasFlag(RegexOptions.IgnorePatternWhitespace))
                flags |= RureFlags.Space;
            if (options.HasFlag(RegexOptions.Multiline))
                flags |= RureFlags.Multi;
            if (options.HasFlag(RegexOptions.Singleline))
                flags |= RureFlags.Dotnl;
            if (options.HasFlag(RegexOptions.RightToLeft))
                throw new NotSupportedException("IronRure does not support rightmost-first searching");
            if (options.HasFlag(RegexOptions.ECMAScript))
                throw new NotSupportedException("IronRure does not support ECMAScript mode");
            return flags;
        }

        // -----------------------------------------------------------------------
        // IsMatch
        // -----------------------------------------------------------------------

        /// <summary>
        ///   Check for a match in the given string.
        /// </summary>
        public bool IsMatch(string haystack) => IsMatch(haystack, 0);

        /// <summary>
        ///   Check for a match in the given string at the given
        ///   character offset.
        /// </summary>
        public bool IsMatch(string haystack, int offset)
        {
            if (offset < 0 || offset > haystack.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            var byteOffset = Encoding.UTF8.GetByteCount(haystack.AsSpan(0, offset));
            return _pattern.IsMatch(haystack, (uint)byteOffset);
        }

        /// <summary>
        ///   Static version of the simple <see cref="IsMatch(string)" />.
        /// </summary>
        public static bool IsMatch(string haystack, string pattern) =>
            IsMatch(haystack, pattern, RegexOptions.None);

        /// <summary>
        ///   Static version of <see cref="IsMatch(string,int)" /> with options.
        /// </summary>
        public static bool IsMatch(string haystack, string pattern, RegexOptions options) =>
            new Regex(pattern, options).IsMatch(haystack);

        /// <summary>
        ///   Static version of <see cref="IsMatch(string,int)" /> with match timeout.
        /// </summary>
        public static bool IsMatch(string haystack, string pattern, RegexOptions options, TimeSpan matchTimeout) =>
            IsMatch(haystack, pattern, options);

        // -----------------------------------------------------------------------
        // Match
        // -----------------------------------------------------------------------

        /// <summary>
        ///   Searches the input string for the first occurrence of this regular
        ///   expression.
        /// </summary>
        public Match Match(string input) => Match(input, 0);

        /// <summary>
        ///   Searches the input string for the first occurrence of this regular
        ///   expression, beginning at the specified character position.
        /// </summary>
        public Match Match(string input, int startat)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            var byteOffset = Encoding.UTF8.GetByteCount(input.ToCharArray(), 0, startat);
            return MatchAtByteOffset(input, bytes, byteOffset);
        }

        /// <summary>
        ///   Static version of <see cref="Match(string)" />.
        /// </summary>
        public static Match Match(string input, string pattern) =>
            Match(input, pattern, RegexOptions.None);

        /// <summary>
        ///   Static version of <see cref="Match(string)" /> with options.
        /// </summary>
        public static Match Match(string input, string pattern, RegexOptions options) =>
            new Regex(pattern, options).Match(input);

        /// <summary>
        ///   Static version of <see cref="Match(string)" /> with options and timeout.
        /// </summary>
        public static Match Match(string input, string pattern, RegexOptions options, TimeSpan matchTimeout) =>
            Match(input, pattern, options);

        // -----------------------------------------------------------------------
        // Matches
        // -----------------------------------------------------------------------

        /// <summary>
        ///   Searches the input string for all occurrences of this regular
        ///   expression.
        /// </summary>
        public MatchCollection Matches(string input) => Matches(input, 0);

        /// <summary>
        ///   Searches the input string for all occurrences of this regular
        ///   expression, beginning at the specified character position.
        /// </summary>
        public MatchCollection Matches(string input, int startat)
        {
            var results = new List<Match>();
            var m = Match(input, startat);
            while (m.Success)
            {
                results.Add(m);
                m = m.NextMatch();
            }
            return new MatchCollection(results);
        }

        /// <summary>
        ///   Static version of <see cref="Matches(string)" />.
        /// </summary>
        public static MatchCollection Matches(string input, string pattern) =>
            Matches(input, pattern, RegexOptions.None);

        /// <summary>
        ///   Static version of <see cref="Matches(string)" /> with options.
        /// </summary>
        public static MatchCollection Matches(string input, string pattern, RegexOptions options) =>
            new Regex(pattern, options).Matches(input);

        /// <summary>
        ///   Static version of <see cref="Matches(string)" /> with options and timeout.
        /// </summary>
        public static MatchCollection Matches(string input, string pattern, RegexOptions options, TimeSpan matchTimeout) =>
            Matches(input, pattern, options);

        // -----------------------------------------------------------------------
        // Replace
        // -----------------------------------------------------------------------

        /// <summary>
        ///   Replaces all strings in a specified input string that match a
        ///   regular expression pattern with a specified replacement string.
        /// </summary>
        public string Replace(string input, string replacement) =>
            Replace(input, replacement, -1, 0);

        /// <summary>
        ///   Replaces a specified maximum number of strings in a specified input
        ///   string that match a regular expression pattern with a specified
        ///   replacement string.
        /// </summary>
        public string Replace(string input, string replacement, int count) =>
            Replace(input, replacement, count, 0);

        /// <summary>
        ///   Replaces a specified maximum number of strings beginning at a
        ///   specified character position.
        /// </summary>
        public string Replace(string input, string replacement, int count, int startat)
        {
            return Replace(input, m => ExpandReplacement(replacement, m), count, startat);
        }

        /// <summary>
        ///   Replaces all strings in a specified input string that match a
        ///   regular expression pattern with a string returned by a
        ///   <see cref="MatchEvaluator" /> delegate.
        /// </summary>
        public string Replace(string input, MatchEvaluator evaluator) =>
            Replace(input, evaluator, -1, 0);

        /// <summary>
        ///   Replaces a specified maximum number of strings that match with
        ///   a string returned by a <see cref="MatchEvaluator" /> delegate.
        /// </summary>
        public string Replace(string input, MatchEvaluator evaluator, int count) =>
            Replace(input, evaluator, count, 0);

        /// <summary>
        ///   Replaces a specified maximum number of strings beginning at a
        ///   specified character position.
        /// </summary>
        public string Replace(string input, MatchEvaluator evaluator, int count, int startat)
        {
            var sb = new StringBuilder();
            int pos = 0;
            int replaced = 0;
            var m = Match(input, startat);

            while (m.Success && (count < 0 || replaced < count))
            {
                sb.Append(input, pos, m.Index - pos);
                sb.Append(evaluator(m));
                pos = m.Index + m.Length;
                replaced++;
                m = m.NextMatch();
            }

            sb.Append(input, pos, input.Length - pos);
            return sb.ToString();
        }

        /// <summary>
        ///   Static version of <see cref="Replace(string,string)" />.
        /// </summary>
        public static string Replace(string input, string pattern, string replacement) =>
            Replace(input, pattern, replacement, RegexOptions.None);

        /// <summary>
        ///   Static version of <see cref="Replace(string,string)" /> with options.
        /// </summary>
        public static string Replace(string input, string pattern, string replacement, RegexOptions options) =>
            new Regex(pattern, options).Replace(input, replacement);

        /// <summary>
        ///   Static version of <see cref="Replace(string,string)" /> with options and timeout.
        /// </summary>
        public static string Replace(string input, string pattern, string replacement, RegexOptions options, TimeSpan matchTimeout) =>
            Replace(input, pattern, replacement, options);

        /// <summary>
        ///   Static version of <see cref="Replace(string,MatchEvaluator)" />.
        /// </summary>
        public static string Replace(string input, string pattern, MatchEvaluator evaluator) =>
            Replace(input, pattern, evaluator, RegexOptions.None);

        /// <summary>
        ///   Static version of <see cref="Replace(string,MatchEvaluator)" /> with options.
        /// </summary>
        public static string Replace(string input, string pattern, MatchEvaluator evaluator, RegexOptions options) =>
            new Regex(pattern, options).Replace(input, evaluator);

        /// <summary>
        ///   Static version of <see cref="Replace(string,MatchEvaluator)" /> with options and timeout.
        /// </summary>
        public static string Replace(string input, string pattern, MatchEvaluator evaluator, RegexOptions options, TimeSpan matchTimeout) =>
            Replace(input, pattern, evaluator, options);

        // -----------------------------------------------------------------------
        // Split
        // -----------------------------------------------------------------------

        /// <summary>
        ///   Splits the input string at each position where this regular
        ///   expression finds a match.
        /// </summary>
        public string[] Split(string input) => Split(input, 0, 0);

        /// <summary>
        ///   Splits the input string a specified maximum number of times.
        /// </summary>
        public string[] Split(string input, int count) => Split(input, count, 0);

        /// <summary>
        ///   Splits the input string a specified maximum number of times,
        ///   beginning at a specified character position.
        /// </summary>
        public string[] Split(string input, int count, int startat)
        {
            var results = new List<string>();
            int pos = 0;
            int splitCount = 0;
            var m = Match(input, startat);

            while (m.Success && (count == 0 || splitCount < count - 1))
            {
                results.Add(input.Substring(pos, m.Index - pos));
                pos = m.Index + m.Length;
                splitCount++;
                m = m.NextMatch();
            }

            results.Add(input.Substring(pos));
            return results.ToArray();
        }

        /// <summary>
        ///   Static version of <see cref="Split(string)" />.
        /// </summary>
        public static string[] Split(string input, string pattern) =>
            Split(input, pattern, RegexOptions.None);

        /// <summary>
        ///   Static version of <see cref="Split(string)" /> with options.
        /// </summary>
        public static string[] Split(string input, string pattern, RegexOptions options) =>
            new Regex(pattern, options).Split(input);

        /// <summary>
        ///   Static version of <see cref="Split(string)" /> with options and timeout.
        /// </summary>
        public static string[] Split(string input, string pattern, RegexOptions options, TimeSpan matchTimeout) =>
            Split(input, pattern, options);

        // -----------------------------------------------------------------------
        // Group name helpers
        // -----------------------------------------------------------------------

        /// <summary>
        ///   Returns an array of the names of all the capturing groups in the
        ///   regular expression.  Group 0 (the whole match) is always included.
        ///   Unnamed groups are represented by their numeric string (e.g. <c>"1"</c>).
        /// </summary>
        public string[] GetGroupNames()
        {
            // Build an index-to-name map for named groups
            var indexToName = new Dictionary<int, string>();
            foreach (var name in _pattern.CaptureNames())
            {
                int idx = _pattern[name];
                if (idx >= 0)
                    indexToName[idx] = name;
            }

            var names = new string[_captureCount];
            for (int i = 0; i < _captureCount; i++)
                names[i] = indexToName.TryGetValue(i, out var name) ? name : i.ToString();
            return names;
        }

        /// <summary>
        ///   Returns an array of capturing group numbers that correspond to
        ///   group names in an array.
        /// </summary>
        public int[] GetGroupNumbers()
        {
            var numbers = new int[_captureCount];
            for (int i = 0; i < _captureCount; i++)
                numbers[i] = i;
            return numbers;
        }

        /// <summary>
        ///   Gets the group name that corresponds to the specified group number.
        /// </summary>
        public string GroupNameFromNumber(int i)
        {
            if (i == 0)
                return "0";
            foreach (var name in _pattern.CaptureNames())
            {
                if (_pattern[name] == i)
                    return name;
            }
            return i.ToString();
        }

        /// <summary>
        ///   Returns the group number corresponding to the specified group name.
        /// </summary>
        public int GroupNumberFromName(string name)
        {
            if (name == "0")
                return 0;
            if (int.TryParse(name, out int n))
                return n;
            var idx = _pattern[name];
            return idx < 0 ? -1 : idx;
        }

        // -----------------------------------------------------------------------
        // Escape / Unescape
        // -----------------------------------------------------------------------

        /// <summary>
        ///   Escape any special characters in the pattern so that
        ///   they will be interpreted literally when used as part of
        ///   a regex match.
        /// </summary>
        public static string Escape(string pattern) => SysRegex.Escape(pattern);

        /// <summary>
        ///   Converts any escaped characters in the input string back to their
        ///   unescaped form.
        /// </summary>
        public static string Unescape(string str) => SysRegex.Unescape(str);

        // -----------------------------------------------------------------------
        // Properties
        // -----------------------------------------------------------------------

        /// <summary>
        ///   Gets the time-out interval of the current instance.
        /// </summary>
        public TimeSpan MatchTimeout { get; }

        /// <summary>
        ///   Gets the options that were passed into the Regex constructor.
        /// </summary>
        public RegexOptions Options { get; }

        /// <summary>
        ///   Gets a value indicating whether the regular expression searches
        ///   from right to left.
        /// </summary>
        public bool RightToLeft => Options.HasFlag(RegexOptions.RightToLeft);

        /// <summary>
        ///   Returns the regular expression pattern that was passed into the
        ///   Regex constructor.
        /// </summary>
        public override string ToString() => _patternString;

        // -----------------------------------------------------------------------
        // Internal helpers
        // -----------------------------------------------------------------------

        /// <summary>
        ///   Perform a match starting at a raw UTF-8 byte offset. Used by
        ///   <see cref="Match.NextMatch" />.
        /// </summary>
        internal Match MatchAtByteOffset(string haystack, int byteOffset)
        {
            var bytes = Encoding.UTF8.GetBytes(haystack);
            return MatchAtByteOffset(haystack, bytes, byteOffset);
        }

        private Match MatchAtByteOffset(string haystack, byte[] haystackBytes, int byteOffset)
        {
            // Guard: Rure has undefined behaviour when offset > length.
            // Equality (offset == length) is allowed so patterns like $ can
            // match at the very end of the input.
            if (byteOffset > haystackBytes.Length)
                return DropIn.Match.Empty;

            // Use Find (rure_find) to determine whether there is a match and to
            // obtain the match boundaries.  Captures.Matched has an unresolved
            // marshalling inconsistency with the native library that can cause it
            // to return true even when no match exists, so we use Find as the
            // authoritative no-match check.
            var found = _pattern.Find(haystackBytes, (uint)byteOffset);
            if (!found.Matched)
                return DropIn.Match.Empty;

            int startByte = (int)found.Start;
            int endByte = (int)found.End;

            int startChar = Encoding.UTF8.GetCharCount(haystackBytes, 0, startByte);
            int lengthChar = Encoding.UTF8.GetCharCount(haystackBytes, startByte, endByte - startByte);
            string value = haystack.Substring(startChar, lengthChar);

            // Now fetch captures for group information and dispose after use.
            GroupCollection groups;
            using (var caps = _pattern.Captures(haystackBytes, (uint)byteOffset))
            {
                groups = BuildGroupCollection(haystack, haystackBytes, caps, startChar, lengthChar, value);
            }

            // Calculate where the next search should begin (in bytes).
            // If the match was zero-length we advance by one byte to avoid an
            // infinite loop, but we need to skip over any continuation bytes.
            int nextByteOffset;
            if (endByte > startByte)
            {
                nextByteOffset = endByte;
            }
            else
            {
                // Advance past the current code point (skip UTF-8 continuation bytes)
                nextByteOffset = byteOffset + 1;
                while (nextByteOffset < haystackBytes.Length &&
                       (haystackBytes[nextByteOffset] & 0xC0) == 0x80)
                {
                    nextByteOffset++;
                }
            }

            return new DropIn.Match(true, value, startChar, lengthChar,
                                    groups, this, haystack, nextByteOffset);
        }

        private GroupCollection BuildGroupCollection(
            string haystack, byte[] haystackBytes,
            IronRure.Captures caps,
            int matchStartChar, int matchLengthChar, string matchValue)
        {
            // Build name → index mapping.
            // Start with numeric names for all groups (these may be overridden
            // below for named groups that have an explicit name).
            var nameMap = new Dictionary<string, int>(caps.Length);
            for (int i = 0; i < caps.Length; i++)
                nameMap[i.ToString()] = i;

            // Add named captures, overriding the numeric name where applicable.
            foreach (var name in _pattern.CaptureNames())
            {
                int idx = _pattern[name];
                if (idx >= 0)
                    nameMap[name] = idx;
            }

            var groups = new List<Group>(caps.Length);

            for (int i = 0; i < caps.Length; i++)
            {
                var rawMatch = caps[i];
                if (!rawMatch.Matched)
                {
                    groups.Add(new Group(false, string.Empty, 0, 0));
                    continue;
                }

                int startByte = (int)rawMatch.Start;
                int endByte = (int)rawMatch.End;
                int startChar = Encoding.UTF8.GetCharCount(haystackBytes, 0, startByte);
                int lenChar = Encoding.UTF8.GetCharCount(haystackBytes, startByte, endByte - startByte);
                string val = haystack.Substring(startChar, lenChar);

                groups.Add(new Group(true, val, startChar, lenChar));
            }

            return new GroupCollection(groups, nameMap);
        }

        /// <summary>
        ///   Expand a replacement string, substituting group references such as
        ///   <c>$0</c>, <c>$1</c>, <c>${name}</c>, and <c>$$</c>.
        /// </summary>
        internal static string ExpandReplacement(string replacement, Match match)
        {
            var sb = new StringBuilder(replacement.Length);
            int i = 0;
            while (i < replacement.Length)
            {
                char c = replacement[i];
                if (c == '$' && i + 1 < replacement.Length)
                {
                    char next = replacement[i + 1];
                    if (next == '$')
                    {
                        sb.Append('$');
                        i += 2;
                    }
                    else if (next == '{')
                    {
                        int close = replacement.IndexOf('}', i + 2);
                        if (close >= 0)
                        {
                            string name = replacement.Substring(i + 2, close - i - 2);
                            var g = int.TryParse(name, out int gn)
                                ? match.Groups[gn]
                                : match.Groups[name];
                            if (g.Success)
                                sb.Append(g.Value);
                            i = close + 1;
                        }
                        else
                        {
                            sb.Append(c);
                            i++;
                        }
                    }
                    else if (char.IsDigit(next))
                    {
                        int j = i + 1;
                        while (j < replacement.Length && char.IsDigit(replacement[j]))
                            j++;
                        if (int.TryParse(replacement.Substring(i + 1, j - i - 1), out int groupNum))
                        {
                            var g = match.Groups[groupNum];
                            if (g.Success)
                                sb.Append(g.Value);
                        }
                        i = j;
                    }
                    else
                    {
                        sb.Append(c);
                        i++;
                    }
                }
                else
                {
                    sb.Append(c);
                    i++;
                }
            }
            return sb.ToString();
        }
    }
}
