using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Tekla.Structures.Geometry3d;
using Xunit;
using static Tekla.Extension.Tests.TestHelpers;

namespace Tekla.Extension.Tests
{
    public class OBBExtensionTests
    {
        private static OBB CreateAxisAlignedOBB(Point center, double extentX, double extentY, double extentZ)
        {
            return new OBB(center,
                new Vector(1, 0, 0), new Vector(0, 1, 0), new Vector(0, 0, 1),
                extentX, extentY, extentZ);
        }

        [Fact]
        public void CombineOBBs_TwoOBBs_ReturnsCombined()
        {
            var obb1 = CreateAxisAlignedOBB(new Point(0, 0, 0), 5, 5, 5);
            var obb2 = CreateAxisAlignedOBB(new Point(10, 0, 0), 5, 5, 5);
            var combined = new List<OBB> { obb1, obb2 }.CombineOBBs();
            combined.Should().NotBeNull();
        }

        [Fact]
        public void CombineOBBs_EmptyCollection_ReturnsDefaultOBB()
        {
            var combined = new List<OBB>().CombineOBBs();
            combined.Should().NotBeNull();
        }

        [Fact]
        public void CombineOBBs_SingleOBB_ReturnsSameOBB()
        {
            var obb = CreateAxisAlignedOBB(new Point(5, 5, 5), 10, 10, 10);
            var combined = new List<OBB> { obb }.CombineOBBs();
            combined.Should().NotBeNull();
            AssertPointEqual(combined.Center, 5, 5, 5);
        }

        [Fact]
        public void GetMaximumPoint_BugDocumentation_ReturnsMaxValue()
        {
            // Bug: GetMaximumPoint starts with MaxPoint (double.MaxValue) and uses (x > y) comparer.
            // No vertex will ever be > double.MaxValue, so it always returns MaxPoint unchanged.
            // Should start with MinPoint instead.
            var obb = CreateAxisAlignedOBB(new Point(5, 5, 5), 5, 5, 5);
            var max = obb.GetMaximumPoint();
            max.X.Should().Be(double.MaxValue);
            max.Y.Should().Be(double.MaxValue);
            max.Z.Should().Be(double.MaxValue);
        }

        [Fact]
        public void GetMinimumPoint_ReturnsCorrectMinCorner()
        {
            // GetMinimumPoint starts with MaxPoint and uses (x < y) comparer - this one works correctly.
            var obb = CreateAxisAlignedOBB(new Point(5, 5, 5), 5, 5, 5);
            var min = obb.GetMinimumPoint();
            AssertPointEqual(min, 0, 0, 0);
        }
    }
}
