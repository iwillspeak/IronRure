using System;
using System.Collections;
using System.Collections.Generic;

namespace IronRure.DropIn
{
    /// <summary>
    ///   Returns the set of groups matched by the regular expression, indexed
    ///   both by name and number.
    /// </summary>
    public class GroupCollection : ICollection<Group>
    {
        private readonly IList<Group> _groups;
        private readonly IDictionary<string, int> _nameMap;

        internal GroupCollection(IList<Group> groups, IDictionary<string, int> nameMap)
        {
            _groups = groups;
            _nameMap = nameMap;
        }

        /// <summary>
        ///   Get a group by its index.
        /// </summary>
        public Group this[int groupnum] =>
            groupnum >= 0 && groupnum < _groups.Count
                ? _groups[groupnum]
                : new Group(false, string.Empty, 0, 0);

        /// <summary>
        ///   Get a group by its name. Returns an unsuccessful group if not found.
        ///   Numeric strings (e.g. <c>"0"</c>, <c>"1"</c>) are resolved to the
        ///   corresponding numbered group.
        /// </summary>
        public Group this[string groupname]
        {
            get
            {
                if (_nameMap.TryGetValue(groupname, out int idx))
                    return this[idx];
                // Fall back to numeric index if the name parses as a non-negative integer
                if (int.TryParse(groupname, out int n) && n >= 0)
                    return this[n];
                return new Group(false, string.Empty, 0, 0);
            }
        }

        /// <summary>Gets the number of groups in the collection.</summary>
        public int Count => _groups.Count;

        bool ICollection<Group>.IsReadOnly => true;

        /// <inheritdoc />
        public IEnumerator<Group> GetEnumerator() => _groups.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _groups.GetEnumerator();

        void ICollection<Group>.Add(Group item) =>
            throw new NotSupportedException();

        void ICollection<Group>.Clear() =>
            throw new NotSupportedException();

        bool ICollection<Group>.Contains(Group item) => _groups.Contains(item);

        void ICollection<Group>.CopyTo(Group[] array, int arrayIndex) =>
            _groups.CopyTo(array, arrayIndex);

        bool ICollection<Group>.Remove(Group item) =>
            throw new NotSupportedException();
    }
}
