namespace SharpExperiments.Tests.BloomFilters;
using SharpExperiments.BloomFilters;
using Xunit;
using FluentAssertions;

public class StandardBloomFilterTests
{
    [Fact]
    public void StandardBloomFilter_MightContain_IsCorrect()
    {
        const string c_element = "apples";
        const string c_absent = "kiwis";
        StandardBloomFilter<string> sbf = new(100, 0.001);
        sbf.Add(c_element);
        sbf.GetCurrentFilterSize().Should().BeGreaterThanOrEqualTo(1);
        sbf.MightContain(c_element).Should().BeTrue();
        sbf.MightContain(c_absent).Should().BeFalse();
    }
}