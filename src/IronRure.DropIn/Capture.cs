namespace IronRure.DropIn
{
    /// <summary>
    ///   Represents the results from a single successful subexpression capture.
    /// </summary>
    public class Capture
    {
        internal Capture(string value, int index, int length)
        {
            Value = value;
            Index = index;
            Length = length;
        }

        /// <summary>
        ///   Gets the captured substring from the input string.
        /// </summary>
        public string Value { get; }

        /// <summary>
        ///   The position in the original string where the first character
        ///   of the captured substring was found.
        /// </summary>
        public int Index { get; }

        /// <summary>
        ///   Gets the length of the captured substring.
        /// </summary>
        public int Length { get; }

        /// <summary>
        ///   Returns the captured substring from the input string.
        /// </summary>
        public override string ToString() => Value;
    }
}
