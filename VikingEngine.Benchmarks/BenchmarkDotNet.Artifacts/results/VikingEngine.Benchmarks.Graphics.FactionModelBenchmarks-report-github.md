```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26340.9233)
AMD Ryzen 9 5900X 3.70GHz, 1 CPU, 24 logical and 12 physical cores
.NET SDK 11.0.100-preview.7.26381.103
  [Host] : .NET 10.0.11 (10.0.11, 10.0.1126.37416), X64 RyuJIT x86-64-v3

Job=MediumRun  Toolchain=InProcessEmitToolchain  IterationCount=15  
LaunchCount=2  WarmupCount=10  

```
| Method                                  | Mean     | Error    | StdDev   | Ratio | RatioSD | Gen0    | Gen1    | Allocated | Alloc Ratio |
|---------------------------------------- |---------:|---------:|---------:|------:|--------:|--------:|--------:|----------:|------------:|
| Legacy_50FactionsEliminated_NoCleanup   | 79.39 μs | 3.305 μs | 4.947 μs |  1.00 |    0.09 | 68.9697 | 53.4668 |    1.1 MB |        1.00 |
| Modern_50FactionsEliminated_ClearModels | 90.23 μs | 4.255 μs | 6.369 μs |  1.14 |    0.10 | 70.4346 | 56.6406 |   1.12 MB |        1.02 |
