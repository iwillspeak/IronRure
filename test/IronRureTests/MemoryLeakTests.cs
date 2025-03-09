using System;
using Xunit;

namespace IronRure.Tests;

public class MemoryLeakTests
{
    [Fact]
    public void MemoryLeakTest()
    {
        // Additionally, verify that a temporary Regex is collected.  
        WeakReference weak = new(new Regex("pattern"));

        // Force a full GC to get a clean baseline.  
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        long memoryBefore = GC.GetTotalMemory(true);

        // Run a loop that repeatedly creates and disposes objects.  
        for (int i = 0; i < 1000; i++)
        {
            // Test with Options and Regex.  
            using (Options opts = new Options().WithSize(512).WithDfaSize(512))
            {
                using Regex regex = new(@"(\w+)", opts);
                // Perform some operations to simulate real usage.  
                bool isMatch = regex.IsMatch("test string for memory leak");
                var matches = regex.FindAll("test string for memory leak");
            }

            // Test with RegexSet.  
            using (RegexSet regexSet = new("foo+", "[0-9]+"))
            {
                bool isMatch = regexSet.IsMatch("foo 123");
                SetMatch match = regexSet.Matches("foo 123");
            }

            // Create a Regex instance wrapped in a WeakReference.  
            // No further strong reference is kept, so it should be collectible.  
            _ = new WeakReference(new Regex("pattern"));
        }

        // Force garbage collection after the loop.  
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        long memoryAfter = GC.GetTotalMemory(true);

        // Allow a margin (here set to 2MB, adjust as needed).  
        const long margin = 2 * 1024 * 1024; // 2MB  

        Assert.InRange(memoryAfter, memoryBefore - margin, memoryBefore + margin);

        // Ensure the weak reference is collected.  
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        Assert.False(weak is { IsAlive: true }, "The Regex object should have been collected.");
    }
}
