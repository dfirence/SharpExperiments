```

BenchmarkDotNet v0.14.0, Debian GNU/Linux 12 (bookworm)
Intel Core i7-6770HQ CPU 2.60GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET SDK 8.0.200
  [Host]   : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT AVX2
  .NET 6.0 : .NET 6.0.36 (6.0.3624.51421), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT AVX2


```
| Method                  | Job      | Runtime  | UrlLength | Mean       | Error    | StdDev   | Allocated |
|------------------------ |--------- |--------- |---------- |-----------:|---------:|---------:|----------:|
| **Murmur3_UTF8_CreateHash** | **.NET 6.0** | **.NET 6.0** | **500**       |   **136.1 ns** |  **0.58 ns** |  **0.55 ns** |         **-** |
| Murmur3_UTF8_CreateHash | .NET 8.0 | .NET 8.0 | 500       |   116.9 ns |  0.38 ns |  0.35 ns |         - |
| **Murmur3_UTF8_CreateHash** | **.NET 6.0** | **.NET 6.0** | **2000**      |   **498.5 ns** |  **0.77 ns** |  **0.72 ns** |         **-** |
| Murmur3_UTF8_CreateHash | .NET 8.0 | .NET 8.0 | 2000      |   422.2 ns |  0.87 ns |  0.81 ns |         - |
| **Murmur3_UTF8_CreateHash** | **.NET 6.0** | **.NET 6.0** | **2083**      |   **515.8 ns** |  **1.94 ns** |  **1.81 ns** |         **-** |
| Murmur3_UTF8_CreateHash | .NET 8.0 | .NET 8.0 | 2083      |   476.7 ns |  1.50 ns |  1.40 ns |         - |
| **Murmur3_UTF8_CreateHash** | **.NET 6.0** | **.NET 6.0** | **8192**      | **1,964.2 ns** |  **3.44 ns** |  **2.87 ns** |         **-** |
| Murmur3_UTF8_CreateHash | .NET 8.0 | .NET 8.0 | 8192      | 1,814.8 ns |  4.60 ns |  4.30 ns |         - |
| **Murmur3_UTF8_CreateHash** | **.NET 6.0** | **.NET 6.0** | **16384**     | **3,891.4 ns** | **19.82 ns** | **18.54 ns** |         **-** |
| Murmur3_UTF8_CreateHash | .NET 8.0 | .NET 8.0 | 16384     | 3,599.8 ns |  5.35 ns |  4.47 ns |         - |
