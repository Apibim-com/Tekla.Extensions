using System;
using FluentAssertions;
using Tekla.Structures.Geometry3d;
using Xunit;
using static Tekla.Extension.Tests.TestHelpers;

namespace Tekla.Extension.Tests
{
    public class LineExtensionTests
    {
        [Fact]
        public void RotatePointAroundAxis_90Degrees_AroundZ()
        {
            var startDir = new Vector(1, 0, 0);
            var center = new Point(0, 0, 0);
            var axis = new Vector(0, 0, 1);
            var result = LineExtension.RotatePointAroundAxis(startDir, center, axis, Math.PI / 2);
            AssertPointEqual(result, 0, 1, 0);
        }

        [Fact]
        public void RotatePointAroundAxis_360Degrees_ReturnsOriginal()
        {
            var startDir = new Vector(1, 0, 0);
            var center = new Point(0, 0, 0);
            var axis = new Vector(0, 0, 1);
            var result = LineExtension.RotatePointAroundAxis(startDir, center, axis, 2 * Math.PI);
            AssertPointEqual(result, 1, 0, 0);
        }

        [Fact]
        public void RotatePointAroundAxis_0Degrees_ReturnsOriginal()
        {
            var startDir = new Vector(5, 3, 0);
            var center = new Point(1, 1, 0);
            var axis = new Vector(0, 0, 1);
            var result = LineExtension.RotatePointAroundAxis(startDir, center, axis, 0);
            AssertPointEqual(result, 6, 4, 0);
        }

        [Fact]
        public void DevideBy_IntoOneSegment_ReturnsSameSegment()
        {
            var seg = new LineSegment(new Point(0, 0, 0), new Point(10, 0, 0));
            var result = seg.DevideBy(1);
            result.Length.Should().Be(1);
            AssertPointEqual(result[0].Point1, 0, 0, 0);
            AssertPointEqual(result[0].Point2, 10, 0, 0);
        }

        [Fact]
        public void DevideBy_IntoTwoSegments_ReturnsEqualHalves()
        {
            var seg = new LineSegment(new Point(0, 0, 0), new Point(10, 0, 0));
            var result = seg.DevideBy(2);
            result.Length.Should().Be(2);
            AssertPointEqual(result[0].Point1, 0, 0, 0);
            AssertPointEqual(result[0].Point2, 5, 0, 0);
            AssertPointEqual(result[1].Point1, 5, 0, 0);
            AssertPointEqual(result[1].Point2, 10, 0, 0);
        }

        [Fact]
        public void DevideBy_IntoThreeSegments()
        {
            var seg = new LineSegment(new Point(0, 0, 0), new Point(9, 0, 0));
            var result = seg.DevideBy(3);
            result.Length.Should().Be(3);
            AssertPointEqual(result[0].Point2, 3, 0, 0);
            AssertPointEqual(result[1].Point2, 6, 0, 0);
            AssertPointEqual(result[2].Point2, 9, 0, 0);
        }

        [Fact]
        public void GetCenterPoint_HorizontalSegment()
        {
            var seg = new LineSegment(new Point(0, 0, 0), new Point(10, 0, 0));
            AssertPointEqual(seg.GetCenterPoint(), 5, 0, 0);
        }

        [Fact]
        public void GetCenterPoint_DiagonalSegment()
        {
            var seg = new LineSegment(new Point(0, 0, 0), new Point(10, 10, 10));
            AssertPointEqual(seg.GetCenterPoint(), 5, 5, 5);
        }

        [Fact]
        public void ToLine_ConvertsLineSegmentToLine()
        {
            var seg = new LineSegment(new Point(0, 0, 0), new Point(10, 0, 0));
            var line = seg.ToLine();
            line.Should().NotBeNull();
            AssertPointEqual(line.Origin, 0, 0, 0);
        }
    }
}
