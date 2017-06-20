using System;
using System.Collections;
using System.Collections.Generic;

namespace IronRure
{
    internal class CaptureNamesEnumerable : IEnumerable<string>
    {
        private readonly Regex _regex;

        public CaptureNamesEnumerable(Regex regex)
        {
            _regex = regex;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return new CaptureNamesEnumerator(_regex);
        }

        IEnumerator IEnumerable.GetEnumerator() =>
            (IEnumerator)GetEnumerator();
    }
}
