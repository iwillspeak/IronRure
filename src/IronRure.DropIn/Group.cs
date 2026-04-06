namespace IronRure.DropIn
{
    /// <summary>
    ///   Represents the results from a single capturing group.
    /// </summary>
    public class Group : Capture
    {
        internal Group(bool success, string value, int index, int length)
            : base(value, index, length)
        {
            Success = success;
        }

        /// <summary>
        ///   Gets a value indicating whether the match is successful.
        /// </summary>
        public bool Success { get; }

        /// <summary>
        ///   Returns a <see cref="Group" /> object equivalent to the one supplied
        ///   suitable for sharing between threads.
        /// </summary>
        public static Group Synchronized(Group inner) => inner;
    }
}
