using System.Collections.Generic;
using FluentAssertions;
using Tekla.Structures.Geometry3d;
using Xunit;

namespace Tekla.Extension.Tests
{
    public class GeometryExtensionTests
    {
        [Fact]
        public void IsPointInLineSegment3d_PointOnSegment_ReturnsTrue()
        {
            var seg = new LineSegment(new Point(0, 0, 0), new Point(10, 0, 0));
            GeometryExtension.IsPointInLineSegment3d(seg, new Point(5, 0, 0)).Should().BeTrue();
        }

        [Fact]
        public void IsPointInLineSegment3d_PointAtStart_ReturnsTrue()
        {
            var seg = new LineSegment(new Point(0, 0, 0), new Point(10, 0, 0));
            GeometryExtension.IsPointInLineSegment3d(seg, new Point(0, 0, 0)).Should().BeTrue();
        }

        [Fact]
        public void IsPointInLineSegment3d_PointAtEnd_ReturnsTrue()
        {
            var seg = new LineSegment(new Point(0, 0, 0), new Point(10, 0, 0));
            GeometryExtension.IsPointInLineSegment3d(seg, new Point(10, 0, 0)).Should().BeTrue();
        }

        [Fact]
        public void IsPointInLineSegment3d_PointOffSegment_ReturnsFalse()
        {
            var seg = new LineSegment(new Point(0, 0, 0), new Point(10, 0, 0));
            GeometryExtension.IsPointInLineSegment3d(seg, new Point(5, 5, 0)).Should().BeFalse();
        }

        [Fact]
        public void IsPointInLineSegment3d_PointBeyondEnd_ReturnsFalse()
        {
            var seg = new LineSegment(new Point(0, 0, 0), new Point(10, 0, 0));
            GeometryExtension.IsPointInLineSegment3d(seg, new Point(15, 0, 0)).Should().BeFalse();
        }

        [Fact]
        public void IsPointInLineSegment3d_PointBeforeStart_ReturnsFalse()
        {
            var seg = new LineSegment(new Point(0, 0, 0), new Point(10, 0, 0));
            GeometryExtension.IsPointInLineSegment3d(seg, new Point(-5, 0, 0)).Should().BeFalse();
        }

        [Fact]
        public void IsPointInsidePolygon_PointInside_ReturnsTrue()
        {
            var polygon = new List<Point>
            {
                new Point(0, 0, 0),
                new Point(10, 0, 0),
                new Point(10, 10, 0),
                new Point(0, 10, 0)
            };
            new Point(5, 5, 0).IsPointInsidePolygon(polygon).Should().BeTrue();
        }

        [Fact]
        public void IsPointInsidePolygon_PointOutside_ReturnsFalse()
        {
            var polygon = new List<Point>
            {
                new Point(0, 0, 0),
                new Point(10, 0, 0),
                new Point(10, 10, 0),
                new Point(0, 10, 0)
            };
            new Point(15, 15, 0).IsPointInsidePolygon(polygon).Should().BeFalse();
        }

        [Fact]
        public void IsPointInsidePolygon_PointOnEdge_IncludeTrue_ReturnsTrue()
        {
            var polygon = new List<Point>
            {
                new Point(0, 0, 0),
                new Point(10, 0, 0),
                new Point(10, 10, 0),
                new Point(0, 10, 0)
            };
            new Point(5, 0, 0).IsPointInsidePolygon(polygon, includeLine: true).Should().BeTrue();
        }

        [Fact]
        public void IsPointInsidePolygon_PointOnEdge_IncludeFalse_MayReturnTrueDueToWindingNumber()
        {
            // With includeLine=false, the explicit edge check is skipped,
            // but the winding number algorithm may still classify edge points as inside.
            // This documents the actual behavior.
            var polygon = new List<Point>
            {
                new Point(0, 0, 0),
                new Point(10, 0, 0),
                new Point(10, 10, 0),
                new Point(0, 10, 0)
            };
            // Point on bottom edge - winding number still sees it as inside
            var result = new Point(5, 0, 0).IsPointInsidePolygon(polygon, includeLine: false);
            result.Should().BeTrue();
        }
    }
}
