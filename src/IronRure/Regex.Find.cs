using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronRure;

public partial class Regex
{
    /// <summary>
    ///     Replace the first match with a literal string.
    /// </summary>
    /// <param name="haystack">The value to replace matches in</param>
    /// <param name="replacement">The value to replace with</param>
    public string Replace(string haystack, string replacement)
    {
        return Replace(haystack, replacement, 1);
    }

    /// <summary>
    ///     Replcace the first byte match with a literal byte array
    /// </summary>
    /// <param name="haystack">The value to replace matches in</param>
    /// <param name="replacement">The value to replace with</param>
    public byte[] Replace(byte[] haystack, byte[] replacement)
    {
        return Replace(haystack, replacement, 1);
    }

    /// <summary>
    ///     Replcace the first byte match with a dynamic string
    /// </summary>
    /// <param name="haystack">The value to replace matches in</param>
    /// <param name="replacement">The match evaluator to replace with</param>
    public string Replace(string haystack, Func<Match, string> replacement)
    {
        return Replace(haystack, replacement, 1);
    }

    /// <summary>
    ///     Replcace the first byte match with a dynamic byte array
    /// </summary>
    /// <param name="haystack">The value to replace matches in</param>
    /// <param name="replacement">The match evaluator to replace with</param>
    public byte[] Replace(byte[] haystack, Func<Match, byte[]> replacement)
    {
        return Replace(haystack, replacement, 1);
    }

    /// <summary>
    ///     Replace All Matches with a literal string.
    /// </summary>
    /// <param name="haystack">The value to replace matches in</param>
    /// <param name="replacement">The value to replace with</param>
    public string ReplaceAll(string haystack, string replacement)
    {
        return Replace(haystack, replacement, -1);
    }

    /// <summary>
    ///     Replace all byte matches with a literal byte array..
    /// </summary>
    /// <param name="haystack">The value to replace matches in</param>
    /// <param name="replacement">The value to replace with</param>
    public byte[] ReplaceAll(byte[] haystack, byte[] replacement)
    {
        return Replace(haystack, replacement, -1);
    }

    /// <summary>
    ///     Replcace the first byte match with a dynamic string
    /// </summary>
    /// <param name="haystack">The value to replace matches in</param>
    /// <param name="replacement">The match evaluator to replace with</param>
    public string ReplaceAll(string haystack, Func<Match, string> replacement)
    {
        return Replace(haystack, replacement, -1);
    }

    /// <summary>
    ///     Replcace the first byte match with a dynamic byte array
    /// </summary>
    /// <param name="haystack">The value to replace matches in</param>
    /// <param name="replacement">The match evaluator to replace with</param>
    public byte[] ReplaceAll(byte[] haystack, Func<Match, byte[]> replacement)
    {
        return Replace(haystack, replacement, -1);
    }

    /// <summary>
    ///     Replace with Count
    /// </summary>
    /// <param name="haystack">The value to replace matches in</param>
    /// <param name="replacement">The value to replace with</param>
    /// <param name="count">The number of matches to replace</param>
    public string Replace(string haystack, string replacement, int count)
    {
        return Encoding.UTF8.GetString(Replace(Encoding.UTF8.GetBytes(haystack),
            Encoding.UTF8.GetBytes(replacement),
            count));
    }

    /// <summary>
    ///     Replace with Count
    /// </summary>
    /// <param name="haystack">The value to replace matches in</param>
    /// <param name="replacement">The value to replace with</param>
    /// <param name="count">The number of matches to replace</param>
    public byte[] Replace(byte[] haystack, byte[] replacement, int count)
    {
        return Replace(haystack, _ => replacement, count);
    }


    /// <summary>
    ///     Replcace the first byte match with a literal byte array
    /// </summary>
    /// <param name="haystack">The value to replace matches in</param>
    /// <param name="replacement">The match evaluator to replace with</param>
    /// <param name="count">The number of matches to replace</param>
    public byte[] Replace(byte[] haystack, Func<Match, byte[]> replacement, int count)
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
            {
                break;
            }
        }

        resultBytes.AddRange(haystack
            .Skip(lastMatch));

        return [.. resultBytes];
    }

    /// <summary>
    ///     Replace the first byte match with a literal byte array
    /// </summary>
    /// <param name="haystack">The value to replace matches in</param>
    /// <param name="replacement">The match evaluator to replace with</param>
    /// <param name="count">The number of matches to replace</param>
    public string Replace(string haystack, Func<Match, string> replacement, int count)
    {
        var result = Replace(Encoding.UTF8.GetBytes(haystack),
            (Func<Match, byte[]>)ByteReplace,
            count);
        return Encoding.UTF8.GetString(result);

        byte[] ByteReplace(Match match)
        {
            return Encoding.UTF8.GetBytes(replacement(match));
        }
    }
}
