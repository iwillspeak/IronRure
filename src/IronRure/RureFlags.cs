using System;

namespace IronRure;

/// <summary>
///   The flags listed below can be used in rure_compile to set the default
///   flags. All flags can otherwise be toggled in the expression itself using
///   standard syntax, e.g., `(?i)` turns case insensitive matching on and `(?-i)`
///   disables it.
/// </summary>
[Flags]
public enum RureFlags
{
    /// <summary> The case insensitive (i) flag.</summary>
    Casei = (1 << 0),
    /// <summary>
    ///   The multi-line matching (m) flag. (^ and $ match new line boundaries.)
    /// </summary>
    Multi = (1 << 1),
    /// <summary> The any character (s) flag. (. matches new line.)</summary>
    Dotnl = (1 << 2),
    /// <summary>
    ///   The greedy swap (U) flag. (e.g., + is ungreedy and +? is greedy.)
    /// </summary>
    SwapGreed = (1 << 3),
    /// <summary> The ignore whitespace (x) flag.</summary>
    Space = (1 << 4),
    /// <summary> The Unicode (u) flag.</summary>
    Unicode = (1 << 5),
}
