```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26340.9233)
AMD Ryzen 9 5900X 3.70GHz, 1 CPU, 24 logical and 12 physical cores
.NET SDK 11.0.100-preview.7.26381.103
  [Host] : .NET 10.0.11 (10.0.11, 10.0.1126.37416), X64 RyuJIT x86-64-v3

Job=MediumRun  Toolchain=InProcessEmitToolchain  IterationCount=15  
LaunchCount=2  WarmupCount=10  

```
| Method                                       | Mean      | Error     | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------------------------- |----------:|----------:|----------:|------:|--------:|----------:|------------:|
| Legacy_5000Soldiers_IndividualMatrixParamSet |  4.237 μs | 0.0858 μs | 0.1284 μs |  1.00 |    0.04 |         - |          NA |
| Modern_5000Soldiers_InstancedBufferPack      | 23.080 μs | 0.8841 μs | 1.3233 μs |  5.45 |    0.35 |         - |          NA |
