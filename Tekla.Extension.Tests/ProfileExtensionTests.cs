using FluentAssertions;
using Tekla.Extension.Tests.TestBase;
using Xunit;

namespace Tekla.Extension.Tests
{
    // LibraryProfileItem / ParametricProfileItem route through ModelInternal,
    // so CDelegateSetter must be set.  With ReturnDefaultStrategy every catalog
    // call returns 0 / false, which means Select() → false and all property
    // lookups return 0.  Tests document that behaviour for unknown profiles.
    public class ProfileExtensionTests : TeklaCatalogTestBase
    {
        private const string UnknownProfile = "XYZZY_DOES_NOT_EXIST";

        // ── IsProfileExist ──────────────────────────────────────────────────
#if TEKLA2020
#else
        [Fact]
        public void IsProfileExist_UnknownProfile_ReturnsFalse()
        {
            ProfileExtension.IsProfileExist(UnknownProfile).Should().BeFalse();
        }

        [Fact]
        public void IsProfileExist_EmptyString_ReturnsFalse()
        {
            ProfileExtension.IsProfileExist(string.Empty).Should().BeFalse();
        }

        // ── GetProfileProperty ──────────────────────────────────────────────

        [Fact]
        public void GetProfileProperty_UnknownProfile_ReturnsZero()
        {
            ProfileExtension.GetProfileProperty(UnknownProfile, "HEIGHT")
                .Should().BeApproximately(0, 0.001);
        }

        [Fact]
        public void GetProfileProperty_UnknownAttribute_ReturnsZero()
        {
            ProfileExtension.GetProfileProperty(UnknownProfile, "NONEXISTENT_ATTR")
                .Should().BeApproximately(0, 0.001);
        }

        // ── GetProfileHeight / GetProfileWidth ──────────────────────────────

        [Fact]
        public void GetProfileHeight_UnknownProfile_ReturnsZero()
        {
            ProfileExtension.GetProfileHeight(UnknownProfile)
                .Should().BeApproximately(0, 0.001);
        }

        [Fact]
        public void GetProfileWidth_UnknownProfile_ReturnsZero()
        {
            ProfileExtension.GetProfileWidth(UnknownProfile)
                .Should().BeApproximately(0, 0.001);
        }

        // ── GetProfileSymbol ────────────────────────────────────────────────

        [Fact]
        public void GetProfileSymbol_UnknownProfile_ReturnsZero()
        {
            ProfileExtension.GetProfileSymbol(UnknownProfile, "h")
                .Should().BeApproximately(0, 0.001);
        }

        // ── GetPlateWidthByProfile ──────────────────────────────────────────

        [Fact]
        public void GetPlateWidthByProfile_UnknownProfile_ReturnsZero()
        {
            ProfileExtension.GetPlateWidthByProfile(UnknownProfile)
                .Should().BeApproximately(0, 0.001);
        }
#endif
    }
}
