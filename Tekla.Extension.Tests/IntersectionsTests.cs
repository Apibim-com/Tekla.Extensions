using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Tekla.Structures.Geometry3d;
using Xunit;
using static Tekla.Extension.Tests.TestHelpers;

namespace Tekla.Extension.Tests
{
    public class IntersectionsTests
    {
        [Fact]
        public void IsLineSegmentsIntersect_CrossingSegments_ReturnsTrue()
        {
            var seg1 = new LineSegment(new Point(0, 0, 0), new Point(10, 10, 0));
            var seg2 = new LineSegment(new Point(10, 0, 0), new Point(0, 10, 0));
            Intersections.IsLineSegmentsIntersect(seg1, seg2, out var point).Should().BeTrue();
            AssertPointEqual(point, 5, 5, 0);
        }

        [Fact]
        public void IsLineSegmentsIntersect_ParallelSegments_ReturnsFalse()
        {
            var seg1 = new LineSegment(new Point(0, 0, 0), new Point(10, 0, 0));
            var seg2 = new LineSegment(new Point(0, 5, 0), new Point(10, 5, 0));
            Intersections.IsLineSegmentsIntersect(seg1, seg2, out var point).Should().BeFalse();
            point.Should().BeNull();
        }

        [Fact]
        public void IsLineSegmentsIntersect_NonIntersectingSegments_ReturnsFalse()
        {
            var seg1 = new LineSegment(new Point(0, 0, 0), new Point(1, 0, 0));
            var seg2 = new LineSegment(new Point(5, 5, 0), new Point(10, 10, 0));
            Intersections.IsLineSegmentsIntersect(seg1, seg2, out _).Should().BeFalse();
        }

        [Fact]
        public void IsLineSegmentsIntersect_TouchingAtEndpoint_ReturnsTrue()
        {
            var seg1 = new LineSegment(new Point(0, 0, 0), new Point(5, 5, 0));
            var seg2 = new LineSegment(new Point(5, 5, 0), new Point(10, 0, 0));
            Intersections.IsLineSegmentsIntersect(seg1, seg2, out var point).Should().BeTrue();
            AssertPointEqual(point, 5, 5, 0);
        }

        [Fact]
        public void GetIntersectionPoints_TwoSegmentCollections_FindsIntersections()
        {
            var segs1 = new List<LineSegment>
            {
                new LineSegment(new Point(0, 5, 0), new Point(10, 5, 0))
            };
            var segs2 = new List<LineSegment>
            {
                new LineSegment(new Point(5, 0, 0), new Point(5, 10, 0))
            };
            var points = Intersections.GetIntersectionPoints(segs1, segs2);
            points.Count.Should().Be(1);
            AssertPointEqual(points.First(), 5, 5, 0);
        }

        [Fact]
        public void GetIntersectionPoints_NoIntersections_ReturnsEmpty()
        {
            var segs1 = new List<LineSegment>
            {
                new LineSegment(new Point(0, 0, 0), new Point(1, 0, 0))
            };
            var segs2 = new List<LineSegment>
            {
                new LineSegment(new Point(0, 5, 0), new Point(1, 5, 0))
            };
            Intersections.GetIntersectionPoints(segs1, segs2).Count.Should().Be(0);
        }

        [Fact]
        public void GetIntersectionPoints_Polygons_FindsAllIntersections()
        {
            // Square polygon from (0,0) to (10,10)
            var polygon1 = new List<Point>
            {
                new Point(0, 0, 0),
                new Point(10, 0, 0),
                new Point(10, 10, 0),
                new Point(0, 10, 0)
            };
            // Square polygon from (5,5) to (15,15)
            var polygon2 = new List<Point>
            {
                new Point(5, 5, 0),
                new Point(15, 5, 0),
                new Point(15, 15, 0),
                new Point(5, 15, 0)
            };
            var points = Intersections.GetIntersectionPoints(polygon1, polygon2);
            points.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void IsLineSegmentsIntersect_PerpendicularCross_FindsCorrectPoint()
        {
            var seg1 = new LineSegment(new Point(0, 5, 0), new Point(10, 5, 0));
            var seg2 = new LineSegment(new Point(3, 0, 0), new Point(3, 10, 0));
            Intersections.IsLineSegmentsIntersect(seg1, seg2, out var point).Should().BeTrue();
            AssertPointEqual(point, 3, 5, 0);
        }
    }
}
