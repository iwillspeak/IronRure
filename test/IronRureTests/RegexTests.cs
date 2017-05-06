using System;
using Xunit;

using IronRure;

namespace IronRureTests
{
    public class RegexTests
    {
        [Fact]
        public void RegexCreate_WithEmptyPattern_Succeeds()
        {
            var regex = new Regex("");
        }
    }
}
