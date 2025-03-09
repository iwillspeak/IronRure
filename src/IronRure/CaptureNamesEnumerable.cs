using System.Collections;
using System.Collections.Generic;

namespace IronRure;

internal class CaptureNamesEnumerable(Regex regex) : IEnumerable<string>
{
    public IEnumerator<string> GetEnumerator()
    {
        return new CaptureNamesEnumerator(regex);
    }

    IEnumerator IEnumerable.GetEnumerator() =>
        (IEnumerator)GetEnumerator();
}
