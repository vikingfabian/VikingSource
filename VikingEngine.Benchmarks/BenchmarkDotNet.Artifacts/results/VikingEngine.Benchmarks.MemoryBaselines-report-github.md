```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26340.9233)
AMD Ryzen 9 5900X 3.70GHz, 1 CPU, 24 logical and 12 physical cores
.NET SDK 11.0.100-preview.7.26381.103
  [Host] : .NET 10.0.11 (10.0.11, 10.0.1126.37416), X64 RyuJIT x86-64-v3

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
| Method                       | Categories         | Mean         | Error      | StdDev     | Ratio | RatioSD | Gen0    | Gen1    | Gen2    | Allocated | Alloc Ratio |
|----------------------------- |------------------- |-------------:|-----------:|-----------:|------:|--------:|--------:|--------:|--------:|----------:|------------:|
| AllocateSubTileChunk_Legacy  | SubTile_ChunkAlloc |  9,623.56 ns | 259.725 ns | 753.508 ns |  1.01 |    0.11 | 35.7056 | 35.7056 | 35.7056 |  114794 B |        1.00 |
| AllocateSubTileChunk_Current | SubTile_ChunkAlloc |  1,325.30 ns |  30.549 ns |  87.157 ns |  0.14 |    0.01 |  3.9043 |       - |       - |   65560 B |        0.57 |
|                              |                    |              |            |            |       |         |         |         |         |           |             |
| MeasureSubTile_LegacySize    | SubTile_Size       |     18.98 ns |   0.408 ns |   0.757 ns |  1.00 |    0.06 |       - |       - |       - |         - |          NA |
| MeasureSubTile_CurrentSize   | SubTile_Size       |     19.37 ns |   0.415 ns |   0.820 ns |  1.02 |    0.06 |       - |       - |       - |         - |          NA |
|                              |                    |              |            |            |       |         |         |         |         |           |             |
| AllocateTileChunk_Legacy     | Tile_ChunkAlloc    | 18,544.46 ns | 364.833 ns | 894.941 ns |  1.00 |    0.07 | 76.9043 | 76.9043 | 76.9043 |  246012 B |        1.00 |
| AllocateTileChunk_Current    | Tile_ChunkAlloc    | 11,268.30 ns | 225.204 ns | 465.086 ns |  0.61 |    0.04 | 43.4723 | 43.4723 | 43.4723 |  139417 B |        0.57 |
|                              |                    |              |            |            |       |         |         |         |         |           |             |
| MeasureTile_LegacySize       | Tile_Size          |     27.80 ns |   0.578 ns |   1.071 ns |  1.00 |    0.05 |       - |       - |       - |         - |          NA |
| MeasureTile_CurrentSize      | Tile_Size          |     27.53 ns |   0.553 ns |   0.793 ns |  0.99 |    0.05 |       - |       - |       - |         - |          NA |
