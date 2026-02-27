using FluentAssertions;
using Tekla.Structures.Geometry3d;
using Xunit;
using static Tekla.Extension.Tests.TestHelpers;

namespace Tekla.Extension.Tests
{
    public class VectorExtensionTests
    {
        [Fact]
        public void X_ReturnsUnitX()
        {
            AssertVectorEqual(VectorExtension.X, 1, 0, 0);
        }

        [Fact]
        public void Y_ReturnsUnitY()
        {
            AssertVectorEqual(VectorExtension.Y, 0, 1, 0);
        }

        [Fact]
        public void Z_ReturnsUnitZ()
        {
            AssertVectorEqual(VectorExtension.Z, 0, 0, 1);
        }

        [Fact]
        public void Null_ReturnsZeroVector()
        {
            AssertVectorEqual(VectorExtension.Null, 0, 0, 0);
        }

        [Fact]
        public void Nan_ReturnsNanVector()
        {
            var nan = VectorExtension.Nan;
            double.IsNaN(nan.X).Should().BeTrue();
            double.IsNaN(nan.Y).Should().BeTrue();
            double.IsNaN(nan.Z).Should().BeTrue();
        }

        [Fact]
        public void Add_TwoVectors_ReturnsSumVector()
        {
            var v = new Vector(1, 2, 3);
            var p = new Point(4, 5, 6);
            AssertVectorEqual(v.Add(p), 5, 7, 9);
        }

        [Fact]
        public void Subtract_TwoVectors_ReturnsDifferenceVector()
        {
            var v = new Vector(4, 5, 6);
            var p = new Point(1, 2, 3);
            AssertVectorEqual(v.Subtract(p), 3, 3, 3);
        }

        [Fact]
        public void Negative_ReturnsNegatedVector()
        {
            var v = new Vector(1, -2, 3);
            AssertVectorEqual(v.Negative(), -1, 2, -3);
        }

        [Fact]
        public void ToPoint_ConvertsCorrectly()
        {
            var v = new Vector(1, 2, 3);
            var p = v.ToPoint();
            AssertPointEqual(p, 1, 2, 3);
        }

        [Fact]
        public void ProjectPointToVector_ProjectsCorrectly()
        {
            var point = new Point(3, 4, 0);
            var vector = new Vector(1, 0, 0);
            var projected = point.ProjectPointToVector(vector);
            AssertPointEqual(projected, 3, 0, 0);
        }

        [Fact]
        public void ProjectPointToVector_DiagonalProjection()
        {
            var point = new Point(2, 2, 0);
            var vector = new Vector(1, 1, 0);
            var projected = point.ProjectPointToVector(vector);
            AssertPointEqual(projected, 2, 2, 0);
        }

        [Fact]
        public void Round_RoundsToSpecifiedDigits()
        {
            var v = new Vector(1.12345, 2.6789, 3.5);
            v.Round(2);
            AssertVectorEqual(v, 1.12, 2.68, 3.5);
        }

        [Fact]
        public void Round_DefaultDigits()
        {
            var v = new Vector(1.12345, 2.6789, 3.999);
            v.Round();
            AssertVectorEqual(v, 1.12, 2.68, 4.0);
        }

        [Fact]
        public void GetLengthSquared_UnitVector_ReturnsOne()
        {
            VectorExtension.X.GetLengthSquared().Should().BeApproximately(1.0, Tolerance);
        }

        [Fact]
        public void GetLengthSquared_ArbitraryVector_ReturnsCorrectValue()
        {
            var v = new Vector(3, 4, 0);
            v.GetLengthSquared().Should().BeApproximately(25.0, Tolerance);
        }

        [Fact]
        public void Translate_MovesPointByVector()
        {
            var point = new Point(1, 2, 3);
            var vector = new Vector(10, 20, 30);
            point.Translate(vector);
            AssertPointEqual(point, 11, 22, 33);
        }

        [Fact]
        public void TransformVector_WithIdentityLikeMatrix()
        {
            var cs = new CoordinateSystem(new Point(0, 0, 0), new Vector(1, 0, 0), new Vector(0, 1, 0));
            var matrix = MatrixFactory.FromCoordinateSystem(cs);
            var v = new Vector(1, 0, 0);
            var result = matrix.TransformVector(v);
            AssertVectorEqual(result, 1, 0, 0);
        }
    }
}
