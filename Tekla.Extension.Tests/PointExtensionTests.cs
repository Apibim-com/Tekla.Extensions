using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using Xunit;
using static Tekla.Extension.Tests.TestHelpers;

namespace Tekla.Extension.Tests
{
    public class PointExtensionTests
    {
        [Fact]
        public void MaxPoint_HasMaxValues()
        {
            var p = PointExtension.MaxPoint;
            p.X.Should().Be(double.MaxValue);
            p.Y.Should().Be(double.MaxValue);
            p.Z.Should().Be(double.MaxValue);
        }

        [Fact]
        public void MinPoint_HasMinValues()
        {
            var p = PointExtension.MinPoint;
            p.X.Should().Be(double.MinValue);
            p.Y.Should().Be(double.MinValue);
            p.Z.Should().Be(double.MinValue);
        }

        [Fact]
        public void GetCenterPoint_ReturnsMidpoint()
        {
            var p1 = new Point(0, 0, 0);
            var p2 = new Point(10, 20, 30);
            AssertPointEqual(p1.GetCenterPoint(p2), 5, 10, 15);
        }

        [Fact]
        public void GetCenterPoint_SamePoints_ReturnsSamePoint()
        {
            var p = new Point(5, 5, 5);
            AssertPointEqual(p.GetCenterPoint(p), 5, 5, 5);
        }

        [Fact]
        public void GetVector_ReturnsDirectionVector()
        {
            var start = new Point(1, 2, 3);
            var end = new Point(4, 6, 8);
            AssertVectorEqual(PointExtension.GetVector(start, end), 3, 4, 5);
        }

        [Fact]
        public void Round_DefaultDigits()
        {
            var p = new Point(1.12345, 2.6789, 3.999);
            p.Round();
            AssertPointEqual(p, 1.12, 2.68, 4.0);
        }

        [Fact]
        public void Round_SpecifiedDigits()
        {
            var p = new Point(1.12345, 2.6789, 3.5);
            p.Round(1);
            AssertPointEqual(p, 1.1, 2.7, 3.5);
        }

        [Fact]
        public void GetNearestPoint_TwoPoints_ReturnsCloser()
        {
            var origin = new Point(0, 0, 0);
            var near = new Point(1, 0, 0);
            var far = new Point(10, 0, 0);
            var result = origin.GetNearestPoint(near, far);
            AssertPointEqual(result, 1, 0, 0);
        }

        [Fact]
        public void GetNearestPoint_Collection_ReturnsClosest()
        {
            var origin = new Point(0, 0, 0);
            var points = new List<Point>
            {
                new Point(10, 0, 0),
                new Point(1, 0, 0),
                new Point(5, 0, 0)
            };
            var result = origin.GetNearestPoint(points);
            AssertPointEqual(result, 1, 0, 0);
        }

        [Fact]
        public void GetRemotePoint_Collection_ReturnsFarthest()
        {
            var origin = new Point(0, 0, 0);
            var points = new List<Point>
            {
                new Point(10, 0, 0),
                new Point(1, 0, 0),
                new Point(5, 0, 0)
            };
            var result = origin.GetRemotePoint(points);
            AssertPointEqual(result, 10, 0, 0);
        }

        [Fact]
        public void ToContourPoint_ConvertsCorrectly()
        {
            var p = new Point(1, 2, 3);
            var cp = p.ToContourPoint();
            cp.X.Should().BeApproximately(1, Tolerance);
            cp.Y.Should().BeApproximately(2, Tolerance);
            cp.Z.Should().BeApproximately(3, Tolerance);
        }

        [Fact]
        public void ToPoint_FromContourPoint_ConvertsCorrectly()
        {
            var cp = new ContourPoint(new Point(4, 5, 6), null);
            var p = cp.ToPoint();
            AssertPointEqual(p, 4, 5, 6);
        }

        [Fact]
        public void ToVector_ConvertsCorrectly()
        {
            var p = new Point(1, 2, 3);
            AssertVectorEqual(p.ToVector(), 1, 2, 3);
        }

        [Fact]
        public void IsNull_NaNPoint_ReturnsFalse_DocumentsBug()
        {
            // Documents known bug: uses == double.NaN instead of double.IsNaN()
            // NaN == NaN is always false in IEEE 754
            var p = new Point(double.NaN, double.NaN, double.NaN);
            p.IsNull().Should().BeFalse();
        }

        [Fact]
        public void IsEmpty_ZeroPoint_ReturnsTrue()
        {
            new Point(0, 0, 0).IsEmpty().Should().BeTrue();
        }

        [Fact]
        public void IsEmpty_NonZeroPoint_ReturnsFalse()
        {
            new Point(1, 0, 0).IsEmpty().Should().BeFalse();
        }

        [Fact]
        public void RoundTo_RoundsToNearestMultiple()
        {
            var p = new Point(17, 23, 38);
            p.RoundTo(10);
            AssertPointEqual(p, 20, 20, 40);
        }

        [Fact]
        public void RoundTo_ExactMultiple_Unchanged()
        {
            var p = new Point(20, 30, 40);
            p.RoundTo(10);
            AssertPointEqual(p, 20, 30, 40);
        }

        [Fact]
        public void CeilingTo_RoundsUp()
        {
            var p = new Point(11, 21, 31);
            p.CeilingTo(10);
            AssertPointEqual(p, 20, 30, 40);
        }

        [Fact]
        public void CeilingTo_ExactMultiple_Unchanged()
        {
            var p = new Point(20, 30, 40);
            p.CeilingTo(10);
            AssertPointEqual(p, 20, 30, 40);
        }

        [Fact]
        public void FloorTo_RoundsDown()
        {
            var p = new Point(19, 29, 39);
            p.FloorTo(10);
            AssertPointEqual(p, 10, 20, 30);
        }

        [Fact]
        public void FloorTo_ExactMultiple_Unchanged()
        {
            var p = new Point(20, 30, 40);
            p.FloorTo(10);
            AssertPointEqual(p, 20, 30, 40);
        }

        [Fact]
        public void ResetX_ReturnsNewPointWithModifiedX()
        {
            var p = new Point(1, 2, 3);
            var result = p.ResetX(99);
            AssertPointEqual(result, 99, 2, 3);
        }

        [Fact]
        public void ResetY_ReturnsNewPointWithModifiedY()
        {
            var p = new Point(1, 2, 3);
            var result = p.ResetY(99);
            AssertPointEqual(result, 1, 99, 3);
        }

        [Fact]
        public void ResetZ_ReturnsNewPointWithModifiedZ()
        {
            var p = new Point(1, 2, 3);
            var result = p.ResetZ(99);
            AssertPointEqual(result, 1, 2, 99);
        }

        [Fact]
        public void GetLineSegmentsOfPolygon_ClosedTriangle_Returns3Segments()
        {
            var points = new List<Point>
            {
                new Point(0, 0, 0),
                new Point(10, 0, 0),
                new Point(5, 10, 0)
            };
            var segments = points.GetLineSegmentsOfPolygon(isClosed: true);
            segments.Count.Should().Be(3);
        }

        [Fact]
        public void GetLineSegmentsOfPolygon_OpenPolyline_Returns2Segments()
        {
            var points = new List<Point>
            {
                new Point(0, 0, 0),
                new Point(10, 0, 0),
                new Point(5, 10, 0)
            };
            var segments = points.GetLineSegmentsOfPolygon(isClosed: false);
            segments.Count.Should().Be(2);
        }

        [Fact]
        public void GetPolygonAABB_ReturnsCorrectBounds()
        {
            var points = new List<Point>
            {
                new Point(1, 2, 3),
                new Point(10, 20, 30),
                new Point(5, 5, 5)
            };
            var aabb = PointExtension.GetPolygonAABB(points);
            AssertPointEqual(aabb.MinPoint, 1, 2, 3);
            AssertPointEqual(aabb.MaxPoint, 10, 20, 30);
        }

        [Fact]
        public void GetMinimumDistance_ReturnsShortestDistanceBetweenConsecutivePoints()
        {
            var points = new List<Point>
            {
                new Point(0, 0, 0),
                new Point(1, 0, 0),
                new Point(5, 0, 0)
            };
            points.GetMinimumDistance().Should().BeApproximately(1.0, Tolerance);
        }

        [Fact]
        public void GetMaximumDistance_ReturnsLongestDistanceBetweenConsecutivePoints()
        {
            var points = new List<Point>
            {
                new Point(0, 0, 0),
                new Point(1, 0, 0),
                new Point(10, 0, 0)
            };
            // Consecutive distances: [0]->[1]=1, [1]->[2]=9, [2]->[0]=10
            points.GetMaximumDistance().Should().BeApproximately(10.0, Tolerance);
        }

        [Fact]
        public void ComparePoints_DefaultComparer_WritesMaxValues()
        {
            var source = new Point(10, 5, 20);
            var target = new Point(5, 10, 15);
            PointExtension.ComparePoints(source, target);
            AssertPointEqual(target, 10, 10, 20);
        }

        [Fact]
        public void ComparePoints_MinComparer_WritesMinValues()
        {
            var source = new Point(3, 15, 8);
            var target = new Point(5, 10, 12);
            PointExtension.ComparePoints(source, target, (x, y) => x < y);
            AssertPointEqual(target, 3, 10, 8);
        }
    }
}
