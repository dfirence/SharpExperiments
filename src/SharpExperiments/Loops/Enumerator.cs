//  Method                            | Cycles  | Mean            | Error         | StdDev        | Gen0   | Allocated |
// |---------------------------------- |-------- |----------------:|--------------:|--------------:|-------:|----------:|
// | GetEnumerator_WhileLoop           | 10      |        24.86 ns |      3.294 ns |      0.510 ns | 0.0048 |      40 B |
// | GetEnumerator_ForLoop             | 10      |        10.40 ns |      1.061 ns |      0.164 ns |      - |         - |
// | GetEnumerator_ForEachLoop         | 10      |        10.61 ns |      0.368 ns |      0.096 ns |      - |         - |
// | GetEnumerator_ForEach_String_Loop | 10      |        18.10 ns |      4.072 ns |      1.058 ns |      - |         - |
// | GetEnumerator_WhileLoop           | 100     |       163.39 ns |      5.241 ns |      1.361 ns | 0.0048 |      40 B |
// | GetEnumerator_ForLoop             | 100     |       115.33 ns |      4.813 ns |      1.250 ns |      - |         - |
// | GetEnumerator_ForEachLoop         | 100     |       117.93 ns |     20.232 ns |      5.254 ns |      - |         - |
// | GetEnumerator_ForEach_String_Loop | 100     |       164.23 ns |      7.087 ns |      1.841 ns |      - |         - |
// | GetEnumerator_WhileLoop           | 1000    |     1,578.30 ns |     55.758 ns |     14.480 ns | 0.0038 |      40 B |
// | GetEnumerator_ForLoop             | 1000    |     1,116.74 ns |     55.154 ns |     14.323 ns |      - |         - |
// | GetEnumerator_ForEachLoop         | 1000    |     1,113.87 ns |     41.032 ns |     10.656 ns |      - |         - |
// | GetEnumerator_ForEach_String_Loop | 1000    |     1,342.23 ns |     62.111 ns |     16.130 ns |      - |         - |
// | GetEnumerator_WhileLoop           | 100000  |   155,641.62 ns |  7,964.785 ns |  2,068.429 ns |      - |      40 B |
// | GetEnumerator_ForLoop             | 100000  |   110,482.11 ns |  3,688.477 ns |    957.886 ns |      - |         - |
// | GetEnumerator_ForEachLoop         | 100000  |   110,209.25 ns |  1,910.328 ns |    496.106 ns |      - |         - |
// | GetEnumerator_ForEach_String_Loop | 100000  |   134,664.88 ns |  5,081.191 ns |  1,319.569 ns |      - |         - |
// | GetEnumerator_WhileLoop           | 1000000 | 1,560,600.73 ns | 76,957.957 ns | 19,985.737 ns |      - |      41 B |
// | GetEnumerator_ForLoop             | 1000000 | 1,112,918.46 ns | 54,899.604 ns | 14,257.253 ns |      - |       1 B |
// | GetEnumerator_ForEachLoop         | 1000000 | 1,106,751.16 ns | 55,820.442 ns | 14,496.391 ns |      - |       1 B |
// | GetEnumerator_ForEach_String_Loop | 1000000 | 1,360,674.02 ns | 42,391.452 ns | 11,008.926 ns |      - |       1 B |
//
//
// # Unroll Factor: 1,0000
//
// | Method                            | Cycles  | Mean     | Error     | StdDev    | Allocated |
// |---------------------------------- |-------- |---------:|----------:|----------:|----------:|
// | GetEnumerator_WhileLoop           | 1000000 | 1.543 us | 0.0578 us | 0.0150 us |         - |
// | GetEnumerator_ForLoop             | 1000000 | 1.103 us | 0.0388 us | 0.0101 us |         - |
// | GetEnumerator_ForEachLoop         | 1000000 | 1.108 us | 0.0649 us | 0.0169 us |         - |
// | GetEnumerator_ForEach_String_Loop | 1000000 | 1.345 us | 0.0522 us | 0.0136 us |         - |
//

namespace SharpExperiments.Iterators;

public class GetEnumeratorWhile
{
    public int current { get; set; }
    public string? currentString { get; set; }
    private List<int>? cycles { get; set; }
    private List<string>? cyclesOfString { get; set; }
    public void Setup(int iterations)
    {
        cycles = new(iterations);
        cyclesOfString = new(iterations);

        for (int n = 0; n < iterations; n++)
        {
            cycles?.Add(n);
            cyclesOfString!.Add($"s-{n}");
        }
    }

    public void RunWhileLoop()
    {
        using (IEnumerator<int> enumerator = cycles!.GetEnumerator())
        {
            while (enumerator!.MoveNext())
            {
                current = enumerator!.Current;
            }
        }
    }

    public void RunForLoop()
    {
        for (int n = 0; n < cycles!.Count; n++)
            current = cycles![n];
    }

    public void RunForEachLoop()
    {
        foreach (int n in cycles!)
            current = n;
    }

    public void RunForEachStringLoop()
    {
        foreach (string s in cyclesOfString!)
            currentString = s;
    }
}