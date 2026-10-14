```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26340.9577)
AMD Ryzen 9 5900X 3.70GHz, 1 CPU, 24 logical and 12 physical cores
.NET SDK 11.0.100-preview.7.26381.103
  [Host] : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v3

Toolchain=InProcessEmitToolchain  IterationCount=6  LaunchCount=1  
WarmupCount=3  

```
| Method                                         | TileCount | Mean       | Error      | StdDev     | Ratio | RatioSD | Gen0    | Gen1    | Allocated | Alloc Ratio |
|----------------------------------------------- |---------- |-----------:|-----------:|-----------:|------:|--------:|--------:|--------:|----------:|------------:|
| **RenderThread_1_Original_SyncIngestion**          | **5**         |   **6.589 μs** |  **1.7119 μs** |  **0.6105 μs** |  **1.01** |    **0.12** |  **1.4114** |  **0.0992** |   **23640 B** |        **1.00** |
| RenderThread_2_Fabian_AsyncPreSynced           | 5         |   1.230 μs |  0.0933 μs |  0.0333 μs |  0.19 |    0.02 |       - |       - |         - |        0.00 |
| RenderThread_3_Refactored_StagedAsyncPreSynced | 5         |   1.441 μs |  0.3407 μs |  0.1215 μs |  0.22 |    0.02 |       - |       - |         - |        0.00 |
| Ingestion_1_Original_Sequential                | 5         |   5.233 μs |  0.8008 μs |  0.2856 μs |  0.80 |    0.08 |  1.0300 |  0.0534 |   17320 B |        0.73 |
| Ingestion_2_Fabian_Parallel_LockContention     | 5         |  38.145 μs |  6.0223 μs |  2.1476 μs |  5.83 |    0.56 |  1.2817 |  0.0610 |   21270 B |        0.90 |
| Ingestion_3_Refactored_Parallel_LockFreeQueue  | 5         |  23.716 μs |  1.5882 μs |  0.5664 μs |  3.62 |    0.31 |  1.6785 |  0.1221 |   27973 B |        1.18 |
| Pipeline_1_Original_WorkerGen_RenderSync       | 5         |  44.849 μs |  4.5824 μs |  1.1900 μs |  6.85 |    0.58 |  7.3242 |  1.3428 |  117184 B |        4.96 |
| Pipeline_2_Fabian_AsyncGenAndSync_Lock         | 5         |  23.314 μs |  5.7058 μs |  2.0347 μs |  3.56 |    0.41 |  7.8430 |  0.6409 |  110815 B |        4.69 |
| Pipeline_3_Refactored_AsyncGenAndSync_Staged   | 5         |  23.635 μs |  3.1777 μs |  1.1332 μs |  3.61 |    0.34 |  7.9956 |  0.7935 |  117079 B |        4.95 |
|                                                |           |            |            |            |       |         |         |         |           |             |
| **RenderThread_1_Original_SyncIngestion**          | **20**        |  **21.159 μs** |  **2.4928 μs** |  **0.8889 μs** |  **1.00** |    **0.05** |  **4.0283** |  **0.5493** |   **67592 B** |        **1.00** |
| RenderThread_2_Fabian_AsyncPreSynced           | 20        |   5.410 μs |  0.6644 μs |  0.2369 μs |  0.26 |    0.01 |       - |       - |         - |        0.00 |
| RenderThread_3_Refactored_StagedAsyncPreSynced | 20        |   5.454 μs |  0.5260 μs |  0.1876 μs |  0.26 |    0.01 |       - |       - |         - |        0.00 |
| Ingestion_1_Original_Sequential                | 20        |  18.248 μs |  1.2121 μs |  0.4322 μs |  0.86 |    0.04 |  2.5024 |  0.2136 |   42328 B |        0.63 |
| Ingestion_2_Fabian_Parallel_LockContention     | 20        | 121.259 μs | 15.0225 μs |  5.3572 μs |  5.74 |    0.32 |  2.8076 |  0.2441 |   46854 B |        0.69 |
| Ingestion_3_Refactored_Parallel_LockFreeQueue  | 20        |  62.445 μs |  1.8692 μs |  0.4854 μs |  2.96 |    0.11 |  4.3945 |  0.6104 |   71324 B |        1.06 |
| Pipeline_1_Original_WorkerGen_RenderSync       | 20        | 125.283 μs |  7.7341 μs |  2.0085 μs |  5.93 |    0.24 | 26.6113 | 10.7422 |  441097 B |        6.53 |
| Pipeline_2_Fabian_AsyncGenAndSync_Lock         | 20        |  90.775 μs | 11.7530 μs |  4.1912 μs |  4.30 |    0.25 | 28.3203 |  4.7607 |  410118 B |        6.07 |
| Pipeline_3_Refactored_AsyncGenAndSync_Staged   | 20        |  84.051 μs | 10.8767 μs |  3.8787 μs |  3.98 |    0.23 | 30.5176 |  6.1035 |  435143 B |        6.44 |
|                                                |           |            |            |            |       |         |         |         |           |             |
| **RenderThread_1_Original_SyncIngestion**          | **50**        |  **48.504 μs** | **10.1451 μs** |  **3.6178 μs** |  **1.00** |    **0.10** |  **7.4463** |  **1.4648** |  **125408 B** |        **1.00** |
| RenderThread_2_Fabian_AsyncPreSynced           | 50        |  13.279 μs |  1.4810 μs |  0.5281 μs |  0.27 |    0.02 |       - |       - |         - |        0.00 |
| RenderThread_3_Refactored_StagedAsyncPreSynced | 50        |  13.582 μs |  3.0276 μs |  0.7863 μs |  0.28 |    0.02 |       - |       - |         - |        0.00 |
| Ingestion_1_Original_Sequential                | 50        |  48.227 μs |  7.9120 μs |  2.8215 μs |  1.00 |    0.08 |  4.4556 |  0.5493 |   75312 B |        0.60 |
| Ingestion_2_Fabian_Parallel_LockContention     | 50        | 330.967 μs | 61.0720 μs | 15.8602 μs |  6.85 |    0.54 |  4.8828 |  0.4883 |   81413 B |        0.65 |
| Ingestion_3_Refactored_Parallel_LockFreeQueue  | 50        | 168.790 μs | 14.3202 μs |  3.7189 μs |  3.50 |    0.24 |  7.8125 |  1.4648 |  130092 B |        1.04 |
| Pipeline_1_Original_WorkerGen_RenderSync       | 50        | 297.900 μs | 38.6926 μs | 13.7981 μs |  6.17 |    0.48 | 62.9883 | 29.7852 | 1058834 B |        8.44 |
| Pipeline_2_Fabian_AsyncGenAndSync_Lock         | 50        | 310.315 μs | 52.8323 μs | 18.8405 μs |  6.43 |    0.55 | 68.3594 | 17.0898 |  990696 B |        7.90 |
| Pipeline_3_Refactored_AsyncGenAndSync_Staged   | 50        | 300.052 μs | 23.2933 μs |  8.3066 μs |  6.21 |    0.43 | 72.7539 | 20.9961 | 1040697 B |        8.30 |
