namespace SharpExperiments.Tests.Strings;
using Xunit;
//using FluentAssertions;

public class StringComparisonTests
{
    private const string c_s1 = "GooneyGooGoo";
    private const string c_s2 = "gooneyGooGoo";

    [Fact]
    public void String_Contains_IgnoreCase_True()
    {
        Assert.True(c_s1.Contains("Goo", StringComparison.OrdinalIgnoreCase), "should-be-true");
    }

    [Fact]
    public void String_EndsWith_IgnoreCase_True()
    {
        Assert.True(c_s1.EndsWith("GooGoo", StringComparison.OrdinalIgnoreCase) == true, "should-be-true");
    }

    [Fact]
    public void String_Equals_IgnoreCase_True()
    {
        Assert.True(c_s1.Equals(c_s2, StringComparison.OrdinalIgnoreCase), "should-be-true");
    }

    [Fact]
    public void String_Substring_IgnoreCase_True()
    {
        Assert.True(c_s1.Substring(0, 3).Equals("Goo", StringComparison.OrdinalIgnoreCase), "should-be-true");
    }

    [Fact]
    public void String_StartsWith_IgnoreCase_True()
    {
        Assert.True(c_s1.StartsWith("Gooney", StringComparison.OrdinalIgnoreCase) == true, "should-be-true");
    }

    //----------------------------------------------------------------------------
    // Span Comparisons
    //----------------------------------------------------------------------------

    [Fact]
    public void Span_Contains_IgnoreCase_True()
    {
        Assert.True(c_s1.AsSpan().Contains("Goo".AsSpan(), StringComparison.OrdinalIgnoreCase), "should-be-true");
    }

    [Fact]
    public void Span_EndsWith_IgnoreCase_True()
    {
        Assert.True(c_s1.AsSpan().EndsWith("GooGoo".AsSpan(), StringComparison.OrdinalIgnoreCase), "should-be-true");
    }

    [Fact]
    public void Span_Equals_IgnoreCase_True()
    {
        Assert.True(c_s1.AsSpan().Equals(c_s2.AsSpan(), StringComparison.OrdinalIgnoreCase), "should-be-true");
    }

    [Fact]
    public void Span_StartsWith_IgnoreCase_True()
    {
        Assert.True(c_s1.AsSpan().StartsWith("Gooney".AsSpan(), StringComparison.OrdinalIgnoreCase), "should-be-true");
    }

    [Fact]
    public void Span_Substring_IgnoreCase_True()
    {
        ReadOnlySpan<char> span = c_s1.AsSpan().Slice(0, 3); // Equivalent to Substring(0,3)

        Assert.True(span.Equals("Goo".AsSpan(), StringComparison.OrdinalIgnoreCase), "should-be-true");
    }
}