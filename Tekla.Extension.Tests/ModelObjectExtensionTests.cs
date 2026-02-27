using FluentAssertions;
using Tekla.Extension.Tests.TestBase;
using Tekla.Structures.Model;
using Xunit;

namespace Tekla.Extension.Tests
{
    // All tests create ModelObject subtypes or call GetReportProperty which
    // routes through the Tekla remoting layer — CDelegateSetter must be set.
    public class ModelObjectExtensionTests : TeklaModelTestBase
    {
        // ── GetReportProperty — return values with no model ─────────────────
        // With GenericDelegateFake<ReturnDefaultStrategy> the model call returns
        // the default bool (false) without modifying the ref value, so each
        // overload returns the initializer value used inside the extension.

        [Fact]
        public void GetReportProperty_StringType_ReturnsEmptyString()
        {
            var beam   = new Beam();
            var result = beam.GetReportProperty<string>("PROFILE");
            result.Should().Be(string.Empty);
        }

        [Fact]
        public void GetReportProperty_IntType_ReturnsIntMinValue()
        {
            var beam   = new Beam();
            var result = beam.GetReportProperty<int>("DRAWING.ID");
            result.Should().Be(int.MinValue);
        }

        [Fact]
        public void GetReportProperty_DoubleType_ReturnsDoubleMinValue()
        {
            var beam   = new Beam();
            var result = beam.GetReportProperty<double>("LENGTH");
            result.Should().Be(double.MinValue);
        }

        [Fact]
        public void GetReportProperty_IsSuccess_ReturnsFalse()
        {
            var beam   = new Beam();
            beam.GetReportProperty<string>("HEIGHT", out bool ok);
            ok.Should().BeFalse();
        }

        [Fact]
        public void GetReportProperty_UnsupportedType_ReturnsDefault()
        {
            // The extension returns default(T) for any type that is not
            // string / int / double and sets isSuccess = false.
            var beam   = new Beam();
            var result = beam.GetReportProperty<object>("ANY", out bool ok);
            result.Should().BeNull();
            ok.Should().BeFalse();
        }

        // ── IsConnectionObject ──────────────────────────────────────────────

        [Fact]
        public void IsConnectionObject_Beam_ReturnsFalse()
        {
            new Beam().IsConnectionObject().Should().BeFalse();
        }

        [Fact]
        public void IsConnectionObject_Weld_ReturnsFalse()
        {
            // Weld is an associative object, not a connection object.
            new Weld().IsConnectionObject().Should().BeFalse();
        }

        // ── IsAssociativeObject ─────────────────────────────────────────────

        [Fact]
        public void IsAssociativeObject_Beam_ReturnsFalse()
        {
            new Beam().IsAssociativeObject().Should().BeFalse();
        }

        [Fact]
        public void IsAssociativeObject_Weld_ReturnsTrue()
        {
            new Weld().IsAssociativeObject().Should().BeTrue();
        }

        [Fact]
        public void IsAssociativeObject_BooleanPart_ReturnsTrue()
        {
            new BooleanPart().IsAssociativeObject().Should().BeTrue();
        }

        [Fact]
        public void IsAssociativeObject_SingleRebar_ReturnsTrue()
        {
            new SingleRebar().IsAssociativeObject().Should().BeTrue();
        }
    }
}
