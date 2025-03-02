namespace SharpExperiments.Tests.Strings;
using Xunit;
//using FluentAssertions;

public class StringComparisonTests
{
    private const string c_s1 = "GooneyGooGoo";
    private const string c_s2 = "gooneyGooGoo";

    [Fact]
    public void String_Equals_IgnoreCase_True()
    {
        Assert.True(c_s1.Equals(c_s2, StringComparison.OrdinalIgnoreCase) == true, "should-be-true");
    }

    [Fact]
    public void Spane_Equals_IgnoreCase_True_NoAlloc()
    {
        Assert.True(c_s1.AsSpan().Equals(c_s2.AsSpan(), StringComparison.OrdinalIgnoreCase), "should-be-true");
    }

}