using FluentAssertions;
using Tekla.Extension.Enums;
using Tekla.Extension.Services;
using Xunit;

namespace Tekla.Extension.Tests
{
    public class ProfileTypeEnumConverterTests
    {
        [Theory]
        [InlineData("B", ProfileType.Plate)]
        [InlineData("I", ProfileType.Ibeam)]
        [InlineData("L", ProfileType.Angle)]
        [InlineData("U", ProfileType.Channel)]
        [InlineData("RU", ProfileType.RoundBar)]
        [InlineData("RO", ProfileType.RoundTube)]
        [InlineData("M", ProfileType.RectangularTube)]
        [InlineData("C", ProfileType.CFChannel)]
        [InlineData("T", ProfileType.Tbeam)]
        [InlineData("Z", ProfileType.Zbeam)]
        public void GetProfileTypeFromString_KnownTypes_ReturnsCorrectEnum(string input, ProfileType expected)
        {
            ProfileTypeEnumConverter.GetProfileTypeFromString(input).Should().Be(expected);
        }

        [Theory]
        [InlineData("")]
        [InlineData("X")]
        [InlineData("unknown")]
        public void GetProfileTypeFromString_UnknownString_ReturnsUnknown(string input)
        {
            ProfileTypeEnumConverter.GetProfileTypeFromString(input).Should().Be(ProfileType.Unknown);
        }

        [Theory]
        [InlineData(ProfileType.Plate, "B")]
        [InlineData(ProfileType.Ibeam, "I")]
        [InlineData(ProfileType.Angle, "L")]
        [InlineData(ProfileType.Channel, "U")]
        [InlineData(ProfileType.RoundBar, "RU")]
        [InlineData(ProfileType.RoundTube, "RO")]
        [InlineData(ProfileType.RectangularTube, "M")]
        [InlineData(ProfileType.CFChannel, "C")]
        [InlineData(ProfileType.Tbeam, "T")]
        [InlineData(ProfileType.Zbeam, "Z")]
        public void GetStringValueFromProfileType_KnownTypes_ReturnsCorrectString(ProfileType input, string expected)
        {
            ProfileTypeEnumConverter.GetStringValueFromProfileType(input).Should().Be(expected);
        }

        [Fact]
        public void GetStringValueFromProfileType_Unknown_ReturnsUnknownString()
        {
            ProfileTypeEnumConverter.GetStringValueFromProfileType(ProfileType.Unknown).Should().Be("Unknown");
        }

        [Theory]
        [InlineData("B")]
        [InlineData("I")]
        [InlineData("L")]
        [InlineData("U")]
        [InlineData("RU")]
        [InlineData("RO")]
        [InlineData("M")]
        [InlineData("C")]
        [InlineData("T")]
        [InlineData("Z")]
        public void RoundTrip_StringToEnumToString_ReturnsOriginal(string input)
        {
            var profileType = ProfileTypeEnumConverter.GetProfileTypeFromString(input);
            var result = ProfileTypeEnumConverter.GetStringValueFromProfileType(profileType);
            result.Should().Be(input);
        }
    }
}
