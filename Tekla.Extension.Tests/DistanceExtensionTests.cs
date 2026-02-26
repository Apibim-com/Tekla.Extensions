using FluentAssertions;
using Xunit;

namespace Tekla.Extension.Tests
{
    public class DistanceExtensionTests
    {
        [Fact]
        public void GetDistances_ThreeSpaceSeparated_ReturnsCorrectCount()
        {
            var result = "100 200 300".GetDistances();
            result.Count.Should().Be(3);
        }

        [Fact]
        public void GetDistances_ThreeValues_AllParsedCorrectly()
        {
            var result = "100 200 300".GetDistances();
            result[0].Should().BeApproximately(100, 0.001);
            result[1].Should().BeApproximately(200, 0.001);
            result[2].Should().BeApproximately(300, 0.001);
        }

        [Fact]
        public void GetDistances_SingleValue_ReturnsSingleElement()
        {
            var result = "150".GetDistances();
            result.Count.Should().Be(1);
            result[0].Should().BeApproximately(150, 0.001);
        }

        [Fact]
        public void GetDistances_FourValues_CountMatchesTokens()
        {
            var result = "10 20 30 40".GetDistances();
            result.Count.Should().Be(4);
        }

        [Fact]
        public void GetDistances_EmptyString_ReturnsEmptyList()
        {
            var result = "".GetDistances();
            result.Count.Should().Be(0);
        }

        [Fact]
        public void GetDistances_EqualSpacing_AllValuesIdentical()
        {
            var result = "50 50 50".GetDistances();
            foreach (var d in result)
                d.Should().BeApproximately(50, 0.001);
        }

        [Fact]
        public void GetDistances_DecimalValue_ParsedCorrectly()
        {
            var result = "100.5".GetDistances();
            result.Count.Should().Be(1);
            result[0].Should().BeApproximately(100.5, 0.001);
        }
    }
}
