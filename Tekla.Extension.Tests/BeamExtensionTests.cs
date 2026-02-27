using FluentAssertions;
using Tekla.Extension.Tests.TestBase;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using Xunit;
using static Tekla.Extension.Tests.TestHelpers;

namespace Tekla.Extension.Tests
{
    // Beam is a ModelObject subtype — inherit TeklaModelTestBase to ensure
    // the ICDelegate fake is in place before any Tekla internals are touched.
    public class BeamExtensionTests : TeklaModelTestBase
    {
        private static Beam CreateBeam(double x1, double y1, double z1,
                                       double x2, double y2, double z2)
        {
            return new Beam
            {
                StartPoint = new Point(x1, y1, z1),
                EndPoint   = new Point(x2, y2, z2)
            };
        }

        // ── GetLineSegment ──────────────────────────────────────────────────

        [Fact]
        public void GetLineSegment_ReturnsSegmentMatchingStartAndEnd()
        {
            var beam = CreateBeam(0, 0, 0, 10, 0, 0);
            var seg  = beam.GetLineSegment();
            AssertPointEqual(seg.Point1, 0,  0, 0);
            AssertPointEqual(seg.Point2, 10, 0, 0);
        }

        [Fact]
        public void GetLineSegment_3DBeam_CorrectEndpoints()
        {
            var beam = CreateBeam(1, 2, 3, 4, 6, 9);
            var seg  = beam.GetLineSegment();
            AssertPointEqual(seg.Point1, 1, 2, 3);
            AssertPointEqual(seg.Point2, 4, 6, 9);
        }

        // ── GetVector ───────────────────────────────────────────────────────

        [Fact]
        public void GetVector_HorizontalBeam_ReturnsXAxisVector()
        {
            var beam = CreateBeam(0, 0, 0, 5, 0, 0);
            AssertVectorEqual(beam.GetVector(), 5, 0, 0);
        }

        [Fact]
        public void GetVector_DiagonalBeam_ReturnsCorrectDifference()
        {
            var beam = CreateBeam(1, 2, 3, 4, 6, 3);
            AssertVectorEqual(beam.GetVector(), 3, 4, 0);
        }

        [Fact]
        public void GetVector_ZeroLengthBeam_ReturnsZeroVector()
        {
            var beam = CreateBeam(5, 5, 5, 5, 5, 5);
            AssertVectorEqual(beam.GetVector(), 0, 0, 0);
        }

        // ── GetBeamLength ───────────────────────────────────────────────────

        [Fact]
        public void GetBeamLength_HorizontalBeam_ReturnsExactLength()
        {
            var beam = CreateBeam(0, 0, 0, 1000, 0, 0);
            beam.GetBeamLength().Should().BeApproximately(1000, Tolerance);
        }

        [Fact]
        public void GetBeamLength_ZeroLength_ReturnsZero()
        {
            var beam = CreateBeam(5, 5, 5, 5, 5, 5);
            beam.GetBeamLength().Should().BeApproximately(0, Tolerance);
        }

        [Fact]
        public void GetBeamLength_3_4_0_Triangle_Returns5()
        {
            // Pythagorean triple: sqrt(3² + 4²) = 5
            var beam = CreateBeam(0, 0, 0, 300, 400, 0);
            beam.GetBeamLength().Should().BeApproximately(500, Tolerance);
        }
    }
}
