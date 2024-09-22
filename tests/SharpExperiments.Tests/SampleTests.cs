//using Xunit;
using FluentAssertions;
using SharpExperiments.Arrays;

namespace SharpExperiments.Tests
{

    /// <summary>
    /// Tests for the SimpleArray class used in the main project under the
    /// namepsace SharpExperiments.Arrays.
    /// </summary>
    public class MySimpleArray
    {
        [Fact]
        public void ShouldBeArray()
        {
            SimpleArray array = new();
            array.Should().BeAssignableTo<SimpleArray>();
        }

        [Fact]
        public void ShouldBeNull()
        {
            SimpleArray array = new();
            array.IsNull().Should().BeTrue();
        }

        [Fact]
        public void ShouldBeSize_10()
        {
            SimpleArray array = new();
            array.CreateArray(10);
            array.IsNull().Should().NotBe(true);
            array.GetSize().Should().Be(10);
        }

        [Fact]
        public void ShouldReturnIEnumerable()
        {
            SimpleArray array = new();
            array.CreateArray(10);
            array.ForIterArrayGenerator()
                .Should()
                .BeAssignableTo<IEnumerable<int>>();
        }
    }
}
