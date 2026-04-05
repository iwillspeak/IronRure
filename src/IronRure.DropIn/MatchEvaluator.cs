namespace IronRure.DropIn
{
    /// <summary>
    ///   Represents the method that is called each time a regular expression
    ///   match is found during a <see cref="Regex.Replace(string,MatchEvaluator)" />
    ///   method operation.
    /// </summary>
    /// <param name="match">The <see cref="Match" /> object that represents a
    ///   single regular expression match during a search.</param>
    public delegate string MatchEvaluator(Match match);
}
