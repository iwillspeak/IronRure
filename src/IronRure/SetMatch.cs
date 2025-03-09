namespace IronRure;

/// <summary>Match information for a <see cref="RegexSet" />.</summary>
/// <remarks>Create a new set <see cref="SetMatch" /> instance.</remarks>
/// <param name="matched">True if any of the expressions matched.</param>
/// <param name="matches">Match information for each pattern in the set.</param>
public readonly struct SetMatch(bool matched, bool[] matches)
{
    /// <summary>
    ///   Did any of the patterns in the set match
    /// </summary>
    public bool Matched { get; } = matched;

    /// <summary>
    ///   Match information for each pattern in the set. These
    ///   are in the same order the patterns were passed when
    ///   compiling the set.
    /// </summary>
    public bool[] Matches { get; } = matches;
}
