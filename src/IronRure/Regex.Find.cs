using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Linq;
using System.Text;

namespace IronRure
{
    public partial class Regex
    {
        /// <summary>
        ///   Replace the first match with a literal string.
        /// </summary>
        /// <param name="haystack">The value to replace matches in</param>
        /// <param name="replacement">The value to replace with</param>
        public string Replace(string haystack, string replacement) =>
            Replacen(haystack, replacement, 1);

        /// <summary>
        ///   Replcace the first byte match with a literal byte array
        /// </summary>
        /// <param name="haystack">The value to replace matches in</param>
        /// <param name="replacement">The value to replace with</param>
        public byte[] Replace(byte[] haystack, byte[] replacement) =>
            Replacen(haystack, replacement, 1);

        /// <summary>
        ///   Replcace the first byte match with a dynamic string
        /// </summary>
        /// <param name="haystack">The value to replace matches in</param>
        /// <param name="replacement">The match evaluator to replace with</param>
        public string Replace(string haystack, Func<Match,string> replacement) =>
            Replacen(haystack, replacement, 1);

        /// <summary>
        ///   Replcace the first byte match with a dynamic byte array
        /// </summary>
        /// <param name="haystack">The value to replace matches in</param>
        /// <param name="replacement">The match evaluator to replace with</param>
        public byte[] Replace(byte[] haystack, Func<Match,byte[]> replacement) =>
            Replacen(haystack, replacement, 1);

        /// <summary>
        ///   Replace All Matches with a literal string.
        /// </summary>
        /// <param name="haystack">The value to replace matches in</param>
        /// <param name="replacement">The value to replace with</param>
        public string ReplaceAll(string haystack, string replacement) =>
            Replacen(haystack, replacement, -1);
            
        /// <summary>
        ///   Replace all byte matches with a literal byte array..
        /// </summary>
        /// <param name="haystack">The value to replace matches in</param>
        /// <param name="replacement">The value to replace with</param>
        public byte[] ReplaceAll(byte[] haystack, byte[] replacement) =>
            Replacen(haystack, replacement, -1);

        /// <summary>
        ///   Replcace the first byte match with a dynamic string
        /// </summary>
        /// <param name="haystack">The value to replace matches in</param>
        /// <param name="replacement">The match evaluator to replace with</param>
        public string ReplaceAll(string haystack, Func<Match,string> replacement) =>
            Replacen(haystack, replacement, -1);

        /// <summary>
        ///   Replcace the first byte match with a dynamic byte array
        /// </summary>
        /// <param name="haystack">The value to replace matches in</param>
        /// <param name="replacement">The match evaluator to replace with</param>
        public byte[] ReplaceAll(byte[] haystack, Func<Match,byte[]> replacement) =>
            Replacen(haystack, replacement, -1);

        /// <summary>
        ///   Replace with Count
        /// </summary>
        /// <param name="haystack">The value to replace matches in</param>
        /// <param name="replacement">The value to replace with</param>
        /// <param name="count">The number of matches to replace</param>
        public string Replacen(string haystack, string replacement, int count) =>
            Encoding.UTF8.GetString(Replacen(Encoding.UTF8.GetBytes(haystack),
                                             Encoding.UTF8.GetBytes(replacement),
                                             count));
        
        /// <summary>
        ///   Replace with Count
        /// </summary>
        /// <param name="haystack">The value to replace matches in</param>
        /// <param name="replacement">The value to replace with</param>
        /// <param name="count">The number of matches to replace</param>
        public byte[] Replacen(byte[] haystack, byte[] replacement, int count) =>
            Replacen(haystack, _ => replacement, count);

            
        /// <summary>
        ///   Replcace the first byte match with a literal byte array
        /// </summary>
        /// <param name="haystack">The value to replace matches in</param>
        /// <param name="replacement">The match evaluator to replace with</param>
        /// <param name="count">The number of matches to replace</param>
        public byte[] Replacen(byte[] haystack, Func<Match,byte[]> replacement, int count)
        {
            var resultBytes = new List<byte>();
            var lastMatch = 0;

            foreach (var match in FindAll(haystack))
            {
                resultBytes.AddRange(haystack
                                     .Skip(lastMatch)
                                     .Take((int)match.Start - lastMatch));
                lastMatch = (int)match.End;
                resultBytes.AddRange(replacement(match));

                // If we have run out of replacements then give up
                if (count != -1 && --count <= 0)
                    break;
            }

            resultBytes.AddRange(haystack
                                 .Skip(lastMatch));

            return resultBytes.ToArray();
        }

        /// <summary>
        ///   Replcace the first byte match with a literal byte array
        /// </summary>
        /// <param name="haystack">The value to replace matches in</param>
        /// <param name="replacement">The match evaluator to replace with</param>
        /// <param name="count">The number of matches to replace</param>
        public string Replacen(string haystack, Func<Match,string> replacement, int count)
        {
            Func<Match, byte[]> byteReplace =
                match => Encoding.UTF8.GetBytes(replacement(match));

            var result = Replacen(Encoding.UTF8.GetBytes(haystack),
                                  byteReplace,
                                  count);
            return Encoding.UTF8.GetString(result);
        }
    }
}
