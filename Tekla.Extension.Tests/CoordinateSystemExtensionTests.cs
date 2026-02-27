using FluentAssertions;
using Tekla.Extension.Tests.TestBase;
using Tekla.Structures.Geometry3d;
using Xunit;
using static Tekla.Extension.Tests.TestHelpers;

namespace Tekla.Extension.Tests
{
    public class CoordinateSystemExtensionTests : TeklaModelTestBase
    {
        [Fact]
        public void GetAxisZ_GlobalCS_ReturnsUnitZ()
        {
            var cs = new CoordinateSystem(new Point(0, 0, 0), new Vector(1, 0, 0), new Vector(0, 1, 0));
            var z = cs.GetAxisZ();
            AssertVectorEqual(z, 0, 0, 1);
        }

        [Fact]
        public void GetAxisZ_RotatedCS_ReturnsCrossProduct()
        {
            var cs = new CoordinateSystem(new Point(0, 0, 0), new Vector(0, 1, 0), new Vector(0, 0, 1));
            var z = cs.GetAxisZ();
            AssertVectorEqual(z, 1, 0, 0);
        }

        [Fact]
        public void MatrixToGlobal_AndMatrixToLocal_RoundTrip()
        {
            var cs = new CoordinateSystem(new Point(100, 200, 300), new Vector(1, 0, 0), new Vector(0, 1, 0));
            var toGlobal = cs.MatrixToGlobal();
            var toLocal = cs.MatrixToLocal();

            var original = new Point(10, 20, 30);
            var local = toLocal.Transform(original);
            var backToGlobal = toGlobal.Transform(local);

            AssertPointEqual(backToGlobal, 10, 20, 30);
        }

        [Fact]
        public void MatrixToLocal_TransformsPointToLocalCoordinates()
        {
            var cs = new CoordinateSystem(new Point(100, 0, 0), new Vector(1, 0, 0), new Vector(0, 1, 0));
            var matrix = cs.MatrixToLocal();
            var transformed = matrix.Transform(new Point(110, 0, 0));
            AssertPointEqual(transformed, 10, 0, 0);
        }
#if TEKLA2020
#else
        [Fact]
        public void ToTransformationPlane_ReturnsNonNull()
        {
            var cs = new CoordinateSystem(new Point(0, 0, 0), new Vector(1, 0, 0), new Vector(0, 1, 0));
            var tp = cs.ToTransformationPlane();
            tp.Should().NotBeNull();
        }
#endif
        [Fact]
        public void ToGeometricPlane_ReturnsNonNull()
        {
            var cs = new CoordinateSystem(new Point(0, 0, 0), new Vector(1, 0, 0), new Vector(0, 1, 0));
            var gp = cs.ToGeometricPlane();
            gp.Should().NotBeNull();
        }

        [Fact]
        public void ToGeometricPlane_HasCorrectOrigin()
        {
            var cs = new CoordinateSystem(new Point(5, 10, 15), new Vector(1, 0, 0), new Vector(0, 1, 0));
            var gp = cs.ToGeometricPlane();
            AssertPointEqual(gp.Origin, 5, 10, 15);
        }
    }
}


