using BenchmarkDotNet.Running;

namespace Alice;

public static class Program
{
    public static void Main()
    {
        BenchmarkRunner.Run<AliceBenchTask>();
    }
}
