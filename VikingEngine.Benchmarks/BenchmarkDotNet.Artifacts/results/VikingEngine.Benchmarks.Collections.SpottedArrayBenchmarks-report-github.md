```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26340.9233)
AMD Ryzen 9 5900X 3.70GHz, 1 CPU, 24 logical and 12 physical cores
.NET SDK 11.0.100-preview.7.26381.103
  [Host] : .NET 10.0.11 (10.0.11, 10.0.1126.37416), X64 RyuJIT x86-64-v3

Job=MediumRun  Toolchain=InProcessEmitToolchain  IterationCount=15  
LaunchCount=2  WarmupCount=10  

```
| Method                                | Mean     | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|-------------------------------------- |---------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| Legacy_BurstThenShrink_NoTrim         | 1.901 μs | 0.0245 μs | 0.0352 μs |  1.00 |    0.03 | 0.4997 |    8.2 KB |        1.00 |
| Modern_BurstThenShrink_WithTrimExcess | 2.045 μs | 0.0497 μs | 0.0729 μs |  1.08 |    0.04 | 0.5112 |   8.38 KB |        1.02 |
