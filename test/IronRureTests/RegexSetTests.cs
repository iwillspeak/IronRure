using System;
using Xunit;

using IronRure;

namespace IronRureTets
{
    public class RegexSetTests
    {
        [Fact]
        public void RegexSet_CreateWithSingleRegex_Succeeds()
        {
            var set = new RegexSet(".+");
        }

        [Fact]
        public void RegexSet_CreateWithMultiplePatterns_Succeeds()
        {
            var set = new RegexSet("hel+o", "world", "0x[a-zA-Z0-9]{4,8}");
        }

        [Fact]
        public void RegexSet_AsIDisposable_ImplementsInterface()
        {
            var set = new RegexSet("");
            var dispo = set as IDisposable;

            Assert.NotNull(dispo);
        }
    }
}