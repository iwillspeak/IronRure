using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Text;

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
    public class Regex : UnmanagedResource
    {
        /// <summary>
        ///   Create a new regex instance from the given pattern.
        /// </summary>
        public Regex(string pattern)
            : this(pattern, IntPtr.Zero, (uint)DefaultFlags)
        {}

        /// <summary>
        ///   Create a new regex instance from the given pattern, and with the
        ///   given regex options applied.
        /// </summary>
        public Regex(string pattern, Options opts)
            : this(pattern, opts.Raw, (uint)DefaultFlags)
        {}

        /// <summary>
        ///   Create a new regex instance from the given pattern and flags.
        /// </summary>
        public Regex(string pattern, RureFlags flags)
            : this(pattern, IntPtr.Zero, (uint)flags)
        {}

        /// <summary>
        ///   Create a new regex instance from the given pattern, with the given
        ///   options applied and with the given flags enabled.
        /// </summary>
        public Regex(string pattern, Options opts, RureFlags flags)
            : this(pattern, opts.Raw, (uint)flags)
        {}

        private Regex(string pattern, IntPtr options, uint flags)
            : base(Compile(pattern, options, flags))
        {}

        /// <summary>
        ///   Compiles the given regex and returns the unmanaged pointer to it.
        /// </summary>
        private static IntPtr Compile(string pattern, IntPtr options, uint flags)
        {
            // Convert the pattern to a utf-8 encoded string.
            var patBytes = Encoding.UTF8.GetBytes(pattern);

            using (var err = new Error())
            {
                // Compile the regex. We get back a raw handle to the underlying
                // Rust object.
                var raw = RureFfi.rure_compile(
                    patBytes,
                    new UIntPtr((uint)patBytes.Length),
                    flags,
                    options,
                    err.Raw);
                
                // If the regex failed to compile find out what the problem was.
                if (raw == IntPtr.Zero)
                    throw new RegexCompilationException(err.Message);

                return raw;
            }
        }

        /// <summary>
        ///   Test if this Regex matches <paramref name="haystack" />, starting
        ///   at the given <paramref name="offset" />.
        /// </summary>
        /// <param name="haystack">The string to search for this pattern</param>
        /// <param name="offset">The offset to start searching at</param>
        public bool IsMatch(string haystack, uint offset)
        {
            var haystackBytes = Encoding.UTF8.GetBytes(haystack);

            return RureFfi.rure_is_match(
                Raw, haystackBytes,
                new UIntPtr((uint)haystackBytes.Length),
                new UIntPtr(offset));
        }

        /// <summary>
        ///   Test if this Regex matches <paramref name="haystack" />
        /// </summary>
        /// <param name="haystack">The UTF8 bytes to search for this pattern</param>
        public bool IsMatch(string haystack) => IsMatch(haystack, 0);

        /// <summary>
        ///   Find the extent of the first match.
        /// </summary>
        /// <param name="haystack">The UTF8 bytes to search for this pattern</param>
        /// <param name="offset">The offset to start searching at</param>
        public Match Find(byte[] haystack, uint offset)
        {
            var matchInfo = new RureMatch();

            var matched = RureFfi.rure_find(
                Raw, haystack,
                new UIntPtr((uint)haystack.Length),
                new UIntPtr(offset),
                out matchInfo);
            
            return new Match(haystack, matched, (uint)matchInfo.start, (uint)matchInfo.end);
        }

        /// <summary>
        ///   Find the extent of the first match.
        /// </summary>
        /// <param name="haystack">The string to search for this pattern</param>
        public Match Find(byte[] haystack) => Find(haystack, 0);

        /// <summary>
        ///   Find the extent of the first match.
        /// </summary>
        /// <param name="haystack">The string to search for this pattern</param>
        /// <param name="offset">The offset to start searching at</param>
        public Match Find(string haystack, uint offset)
        {
            var haystackBytes = Encoding.UTF8.GetBytes(haystack);
            return Find(haystackBytes, offset);
        }

        /// <summary>
        ///   Find the extent of the first match.
        /// </summary>
        /// <param name="haystack">The string to search for this pattern</param>
        public Match Find(string haystack) => Find(haystack, 0);

        /// <summary>
        ///   Find All Matches
        ///   <para>Returns an iterator over each match in the pattern</para>
        /// </summary>
        /// <param name="haystack">The string to search for this pattern</param>
        public IEnumerable<Match> FindAll(byte[] haystack)
        {
            using (var iter =  new MatchIter(this, haystack))
            {
                while (iter.MoveNext())
                {
                    yield return iter.Current;
                }
            }
        }

        /// <summary>
        ///   Find All Matches
        ///   <para>Returns an iterator over each match in the pattern</para>
        /// </summary>
        /// <param name="haystack">The string to search for this pattern</param>
        public IEnumerable<Match> FindAll(string haystack)
        {
            var haystackBytes = Encoding.UTF8.GetBytes(haystack);
            return FindAll(haystackBytes);
        }

        /// <summary>
        ///   Get Capture Index from Name
        /// </summary>
        public int this[string capture] =>
            RureFfi.rure_capture_name_index(Raw, Encoding.UTF8.GetBytes(capture));

        /// <summary>
        ///   Get an enumeration of all the named captures in this pattern.
        /// </summary>
        public IEnumerable<string> CaptureNames()
        {
            return new CaptureNamesEnumerable(this);
        }


        /// <summary>
        ///   Captures - Find the extent of the capturing groups in the pattern
        ///   in a given haystack.
        /// </summary>
        /// <param name="haystack">The string to search for this pattern</param>
        /// <param name="offset">The offset to start searching at</param>
        public Captures Captures(byte[] haystack, uint offset)
        {
            var caps = new Captures(this, haystack);
            var matched = RureFfi.rure_find_captures(Raw,
                                                     haystack,
                                                     new UIntPtr((uint)haystack.Length),
                                                     new UIntPtr(offset),
                                                     caps.Raw);

            caps.Matched = matched;
            return caps;
        }

        /// <summary>
        ///   Captures - Find the extent of the capturing groups in the pattern
        ///   in a given haystack.
        /// </summary>
        /// <param name="haystack">The string to search for this pattern</param>
        public Captures Captures(byte[] haystack) => Captures(haystack, 0);
        
        /// <summary>
        ///   Captures - Find the extent of the capturing groups in the pattern
        ///   in a given haystack.
        /// </summary>
        /// <param name="haystack">The string to search for this pattern</param>
        /// <param name="offset">The offset to start searching at</param>
        public Captures Captures(string haystack, uint offset)
        {
            var haystackBytes = Encoding.UTF8.GetBytes(haystack);
            return Captures(haystackBytes, offset);
        }

        /// <summary>
        ///   Captures - Find the extent of the capturing groups in the pattern
        ///   in a given haystack.
        /// </summary>
        /// <param name="haystack">The string to search for this pattern</param>
        public Captures Captures(string haystack) => Captures(haystack, 0);

        /// <summary>
        ///   Capture All Matches
        ///   <para>
        ///     Returns an iterator containing the capture information
        ///     for each match of the pattern.
        ///   </para>
        /// </summary>
        /// <param name="haystack">The string to search for this pattern</param>
        public IEnumerable<Captures> CaptureAll(byte[] haystack)
        {
            using (var iter =  new CapturesIter(this, haystack))
            {
                while (iter.MoveNext())
                {
                    yield return iter.Current;
                }
            }
        }

        /// <summary>
        ///   Capture All Matches
        ///   <para>
        ///     Returns an iterator containing the capture information
        ///     for each match of the pattern.
        ///   </para>
        /// </summary>
        /// <param name="haystack">The string to search for this pattern</param>
        public IEnumerable<Captures> CaptureAll(string haystack)
        {
            var haystackBytes = Encoding.UTF8.GetBytes(haystack);
            return CaptureAll(haystackBytes);
        }

        protected override void Free(IntPtr resource)
        {
            RureFfi.rure_free(resource);
        }

        /// <summary>
        ///   The default flags for the regex
        /// </summary>
        public static RureFlags DefaultFlags => RureFlags.Unicode;
    }
}
