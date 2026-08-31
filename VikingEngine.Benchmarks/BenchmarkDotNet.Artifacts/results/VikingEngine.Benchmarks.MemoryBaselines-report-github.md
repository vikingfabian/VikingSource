```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26340.9233)
AMD Ryzen 9 5900X 3.70GHz, 1 CPU, 24 logical and 12 physical cores
.NET SDK 11.0.100-preview.7.26381.103
  [Host] : .NET 10.0.11 (10.0.11, 10.0.1126.37416), X64 RyuJIT x86-64-v3

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
| Method                       | Categories         | Mean         | Error      | StdDev     | Median       | Ratio | RatioSD | Gen0    | Gen1    | Gen2    | Allocated | Alloc Ratio |
|----------------------------- |------------------- |-------------:|-----------:|-----------:|-------------:|------:|--------:|--------:|--------:|--------:|----------:|------------:|
| AllocateSubTileChunk_Legacy  | SubTile_ChunkAlloc |  9,539.05 ns | 249.525 ns | 735.730 ns |  9,334.01 ns |  1.01 |    0.11 | 35.7056 | 35.7056 | 35.7056 |  114794 B |        1.00 |
| AllocateSubTileChunk_Current | SubTile_ChunkAlloc |  9,162.33 ns | 178.792 ns | 361.168 ns |  9,106.59 ns |  0.97 |    0.08 | 35.7056 | 35.7056 | 35.7056 |  114818 B |        1.00 |
|                              |                    |              |            |            |              |       |         |         |         |         |           |             |
| MeasureSubTile_LegacySize    | SubTile_Size       |     19.44 ns |   0.423 ns |   0.928 ns |     19.53 ns |  1.00 |    0.07 |       - |       - |       - |         - |          NA |
| MeasureSubTile_CurrentSize   | SubTile_Size       |     20.04 ns |   0.434 ns |   0.896 ns |     19.99 ns |  1.03 |    0.07 |       - |       - |       - |         - |          NA |
|                              |                    |              |            |            |              |       |         |         |         |         |           |             |
| AllocateTileChunk_Legacy     | Tile_ChunkAlloc    | 18,066.77 ns | 358.094 ns | 864.838 ns | 18,056.74 ns |  1.00 |    0.07 | 76.9043 | 76.9043 | 76.9043 |  246013 B |        1.00 |
| AllocateTileChunk_Current    | Tile_ChunkAlloc    | 18,074.56 ns | 361.306 ns | 976.813 ns | 17,903.01 ns |  1.00 |    0.07 | 76.9043 | 76.9043 | 76.9043 |  246012 B |        1.00 |
|                              |                    |              |            |            |              |       |         |         |         |         |           |             |
| MeasureTile_LegacySize       | Tile_Size          |     28.08 ns |   0.596 ns |   1.539 ns |     27.90 ns |  1.00 |    0.08 |       - |       - |       - |         - |          NA |
| MeasureTile_CurrentSize      | Tile_Size          |     28.01 ns |   0.587 ns |   1.116 ns |     27.93 ns |  1.00 |    0.07 |       - |       - |       - |         - |          NA |
