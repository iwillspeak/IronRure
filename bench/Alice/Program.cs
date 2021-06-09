using BenchmarkDotNet.Running;

namespace Alice
{
    public static class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<AliceBenchTask>();
        }
    }
}
