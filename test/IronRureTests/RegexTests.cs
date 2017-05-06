using System;
using System.Threading.Tasks;
using Xunit;

using IronRure;

namespace IronRureTests
{
    public class RegexTests
    {
        [Fact]
        public void Regex_CreateWithEmptyPattern_Succeeds()
        {
            var regex = new Regex("");
        }

        [Fact]
        public void Regex_AsIDsiposable_ImplementsInterface()
        {
            var reg = new Regex(".*");
            var dispo = reg as IDisposable;
            Assert.NotNull(dispo);
        }

        [Fact]
        public void Regex_CreateWithInvalidPattern_ThrowsException()
        {
            Assert.Throws<RegexCompilationException>(() => new Regex(")"));
        }
    }
}
