```

BenchmarkDotNet v0.14.0, Debian GNU/Linux 12 (bookworm)
Intel Core i7-6770HQ CPU 2.60GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET SDK 8.0.200
  [Host]   : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT AVX2
  .NET 6.0 : .NET 6.0.36 (6.0.3624.51421), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT AVX2


```
| Method             | Job      | Runtime  | Mean     | Error    | StdDev   | Allocated |
|------------------- |--------- |--------- |---------:|---------:|---------:|----------:|
| Murmur3_256KB_Hash | .NET 6.0 | .NET 6.0 | 61.88 μs | 0.355 μs | 0.315 μs |         - |
| Murmur3_256KB_Hash | .NET 8.0 | .NET 8.0 | 62.03 μs | 0.164 μs | 0.153 μs |         - |
