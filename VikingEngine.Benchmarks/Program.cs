using BenchmarkDotNet.Running;
using VikingEngine.Benchmarks.Pathfinding;

namespace VikingEngine.Benchmarks;

public class Program
{
    public static void Main(string[] args)
    {
        BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
    }
}