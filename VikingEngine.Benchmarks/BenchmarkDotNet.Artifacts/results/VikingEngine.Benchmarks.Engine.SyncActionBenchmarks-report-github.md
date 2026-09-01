```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26340.9233)
AMD Ryzen 9 5900X 3.70GHz, 1 CPU, 24 logical and 12 physical cores
.NET SDK 11.0.100-preview.7.26381.103
  [Host] : .NET 10.0.11 (10.0.11, 10.0.1126.37416), X64 RyuJIT x86-64-v3

Job=MediumRun  Toolchain=InProcessEmitToolchain  IterationCount=15  
LaunchCount=2  WarmupCount=10  

```
| Method                            | Mean     | Error     | StdDev    | Ratio | RatioSD | Gen0   | Gen1   | Allocated | Alloc Ratio |
|---------------------------------- |---------:|----------:|----------:|------:|--------:|-------:|-------:|----------:|------------:|
| Legacy_QueueAndProcess_100Actions | 1.818 μs | 0.0253 μs | 0.0370 μs |  1.00 |    0.03 | 0.3338 | 0.0019 |   5.47 KB |        1.00 |
| Modern_QueueAndProcess_100Actions | 1.934 μs | 0.0276 μs | 0.0414 μs |  1.06 |    0.03 | 0.1411 |      - |   2.34 KB |        0.43 |
