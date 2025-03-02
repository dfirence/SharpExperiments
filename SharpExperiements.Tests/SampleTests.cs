namespace SharpExperiements.Tests;
using Xunit;
using FluentAssertions;

public class SampleTests
{
    [Fact]
    public void DefaultTest_ShouldPass()
    {
        // Arrange
        var value = 1;

        // Act
        var result = value + 1;

        // Assert
        result.Should().Be(2);
    }
}
