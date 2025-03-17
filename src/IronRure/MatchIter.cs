using System;
using System.Collections;

namespace IronRure;

/// <summary>
///     An enumerator of regex matches. Uses the underlying Rure
///     regex iterator.
/// </summary>
public sealed class MatchIter : RegexIter, IEnumerator
{
    private RureMatch _matchInfo;

    /// <summary>
    ///     An enumerator of regex matches. Uses the underlying Rure
    ///     regex iterator.
    /// </summary>
    /// <param name="pattern">The pattern to search with.</param>
    /// <param name="haystack">The haystack to search.</param>
    public MatchIter(Regex pattern, byte[] haystack) : base(pattern, haystack)
    {
    }

    /// <summary>
    /// The current match.
    /// </summary>
    public Match Current { get; private set; }

    /// <summary>
    /// Gets the current element in the collection.
    /// </summary>
    object IEnumerator.Current => Current;

    /// <summary>
    /// Advances the enumerator to the next element of the collection.
    /// </summary>
    /// <returns>true if the enumerator was successfully advanced to the next element; 
    /// false if the enumerator has passed the end of the collection.</returns>
    public bool MoveNext()
    {
        if (Raw == null || Haystack == null)
        {
            Current = null;
            return false;
        }

        var matched = RureFfi.rure_iter_next(Raw,
            Haystack,
            new UIntPtr((uint)Haystack.Length),
            out _matchInfo);

        if (matched)
        {
            checked
            {
                Current = new Match(Haystack,
                    matched,
                    (uint)_matchInfo.start,
                    (uint)_matchInfo.end);
            }
            return true;
        }

        Current = null;
        return false;
    }

    /// <summary>
    /// Sets the enumerator to its initial position.
    /// </summary>
    public void Reset()
    {
        throw new NotImplementedException();
    }
}
