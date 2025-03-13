using System;
using Xunit;

namespace IronRure.Tests;

public class MemoryLeakTests
{
    [Fact]
    public void MemoryLeakTest()
    {
        // Force a full GC to get a clean baseline.  
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        var memoryBefore = GC.GetTotalMemory(true);

        // Run a loop that repeatedly creates and disposes objects.  
        for (var i = 0; i < 1000; i++)
        {
            // Test with Options and Regex.  
            using (var opts = new Options().WithSize(65536).WithDfaSize(65536))
            {
                using Regex regex = new(@"(\w+)", opts);
                // Perform some operations to simulate real usage.  
                var isMatch = regex.IsMatch("test string for memory leak");
                var matches = regex.FindAll("test string for memory leak");
            }

            // Test with RegexSet.  
            using (RegexSet regexSet = new("foo+", "[0-9]+"))
            {
                var isMatch = regexSet.IsMatch("foo 123");
                var match = regexSet.Matches("foo 123");
            }

            // Create a Regex instance wrapped in a WeakReference.  
            // No further strong reference is kept, so it should be collectible.  
            _ = new WeakReference(new Regex("pattern"));
        }

        var memoryAfter = GC.GetTotalMemory(true);

        // Allow a margin (here set to 2MB, adjust as needed).  
        const long margin = 2 * 1024 * 1024; // 2MB  

        Assert.InRange(memoryAfter, memoryBefore - margin, memoryBefore + margin);
    }
}
