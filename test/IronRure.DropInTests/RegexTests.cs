using System;
using Xunit;

namespace IronRure.DropInTests
{
    using IronRure.DropIn;

    public class RegexTests
    {
        [Fact]
        public void CreateWithSimplePattern()
        {
            var rege = new Regex("hello world");
        }

        [Fact]
        public void CreateWithPatternAndOptions()
        {
            var regex = new Regex("^hello.*world$", RegexOptions.Singleline);
        }

        [Fact]
        public void CreateWithOptionsAndTimeout()
        {
            var regex = new Regex(@"\b[at]\w+", RegexOptions.None, TimeSpan.FromMinutes(1));
        }
    }
}
