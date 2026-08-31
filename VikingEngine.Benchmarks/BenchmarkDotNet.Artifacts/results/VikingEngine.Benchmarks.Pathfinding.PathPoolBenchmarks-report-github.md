```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26340.9233)
AMD Ryzen 9 5900X 3.70GHz, 1 CPU, 24 logical and 12 physical cores
.NET SDK 11.0.100-preview.7.26381.103
  [Host] : .NET 10.0.11 (10.0.11, 10.0.1126.37416), X64 RyuJIT x86-64-v3

Job=MediumRun  Toolchain=InProcessEmitToolchain  IterationCount=15  
LaunchCount=2  WarmupCount=10  

```
| Method                               | Mean      | Error     | StdDev    | Ratio | RatioSD | Gen0   | Gen1   | Allocated | Alloc Ratio |
|------------------------------------- |----------:|----------:|----------:|------:|--------:|-------:|-------:|----------:|------------:|
| LegacyPool_GetAndReturn              |  13.92 ns |  0.181 ns |  0.254 ns |  1.00 |    0.03 | 0.0019 |      - |      32 B |        1.00 |
| ModernPool_Preallocated_GetAndReturn |  14.94 ns |  0.924 ns |  1.383 ns |  1.07 |    0.10 | 0.0019 |      - |      32 B |        1.00 |
| Unpooled_NewInstanceAlloc            | 867.69 ns | 50.284 ns | 75.262 ns | 62.35 |    5.43 | 2.4567 | 0.3052 |   41184 B |    1,287.00 |
