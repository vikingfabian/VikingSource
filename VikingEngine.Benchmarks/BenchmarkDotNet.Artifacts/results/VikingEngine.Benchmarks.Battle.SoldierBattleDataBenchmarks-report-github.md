```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26340.9233)
AMD Ryzen 9 5900X 3.70GHz, 1 CPU, 24 logical and 12 physical cores
.NET SDK 11.0.100-preview.7.26381.103
  [Host] : .NET 10.0.11 (10.0.11, 10.0.1126.37416), X64 RyuJIT x86-64-v3

Job=MediumRun  Toolchain=InProcessEmitToolchain  IterationCount=15  
LaunchCount=2  WarmupCount=10  

```
| Method                                    | Mean     | Error     | StdDev    | Ratio | RatioSD | Gen0   | Gen1   | Allocated | Alloc Ratio |
|------------------------------------------ |---------:|----------:|----------:|------:|--------:|-------:|-------:|----------:|------------:|
| Legacy_EnterExitBattle_100Soldiers        | 2.088 μs | 0.1560 μs | 0.2286 μs |  1.01 |    0.15 | 1.0033 | 0.0305 |   16800 B |        1.00 |
| Modern_Pooled_EnterExitBattle_100Soldiers | 4.160 μs | 0.1683 μs | 0.2518 μs |  2.01 |    0.23 |      - |      - |         - |        0.00 |
