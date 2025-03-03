namespace SharpExperiments.Tests.Hashing;
using SharpExperiments.Hashing;
using Xunit;
using FluentAssertions;


public class Murmur3Tests
{
    private const string c_value = "GooneyGooGoo";

    public Murmur3Tests()
    {
    }

    [Fact]
    public void Murmur3_InputValue_CorrectHexHash()
    {
        // Default Seed of `0` for string GooneyGooGoo
        // - [Online TestTool -ShoreLabs](https://murmurhash.shorelabs.com/)
        string seed_0_expectedValue = "c0250dde63f19e35e1da5e574c1f3f12";
        string seed_42_expectedValue = "9668550ca6f44c4873ef1cf36e5dc3f2";

        Murmur3.GetStringHash(c_value).Should().BeEquivalentTo(seed_0_expectedValue);
        Murmur3.GetStringHash(c_value, 42).Should().BeEquivalentTo(seed_42_expectedValue);
    }
}