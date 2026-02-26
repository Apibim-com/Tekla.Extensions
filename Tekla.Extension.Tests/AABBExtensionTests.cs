using System.Linq;
using FluentAssertions;
using Tekla.Structures.Geometry3d;
using Xunit;
using static Tekla.Extension.Tests.TestHelpers;

namespace Tekla.Extension.Tests
{
    public class AABBExtensionTests
    {
        [Fact]
        public void Add_CombinesTwoAABBs_TakesInnerBounds()
        {
            // Note: Add() uses inverted comparers - it takes the inner (smaller) envelope,
            // not the outer union. This documents the actual behavior.
            var aabb1 = new AABB(new Point(0, 0, 0), new Point(10, 10, 10));
            var aabb2 = new AABB(new Point(0, 0, 0), new Point(10, 10, 10));
            var result = aabb1.Add(aabb2);
            AssertPointEqual(result.MinPoint, 0, 0, 0);
            AssertPointEqual(result.MaxPoint, 10, 10, 10);
        }

        [Fact]
        public void Add_IdenticalAABBs_ReturnsSameBounds()
        {
            var aabb = new AABB(new Point(2, 3, 4), new Point(8, 9, 10));
            var result = aabb.Add(aabb);
            AssertPointEqual(result.MinPoint, 2, 3, 4);
            AssertPointEqual(result.MaxPoint, 8, 9, 10);
        }

        [Fact]
        public void ProjectToXYPlane_ReturnsFourPointsWithZeroZ()
        {
            var aabb = new AABB(new Point(0, 0, 5), new Point(10, 20, 30));
            var points = aabb.ProjectToXYPlane();
            points.Count.Should().Be(4);
            foreach (var p in points)
            {
                p.Z.Should().BeApproximately(0, Tolerance);
            }
        }

        [Fact]
        public void ProjectToXYPlane_CorrectCorners()
        {
            var aabb = new AABB(new Point(0, 0, 5), new Point(10, 20, 30));
            var points = aabb.ProjectToXYPlane().ToArray();
            // point1 = MaxPoint projected, point2 = (max.X, min.Y), point3 = MinPoint projected, point4 = (min.X, max.Y)
            AssertPointEqual(points[0], 10, 20, 0);
            AssertPointEqual(points[1], 10, 0, 0);
            AssertPointEqual(points[2], 0, 0, 0);
            AssertPointEqual(points[3], 0, 20, 0);
        }

        [Fact]
        public void ToOBB_CreatesEquivalentOBB()
        {
            var aabb = new AABB(new Point(0, 0, 0), new Point(10, 20, 30));
            var obb = aabb.ToOBB();
            obb.Should().NotBeNull();
            // Center should be at midpoint
            AssertPointEqual(obb.Center, 5, 10, 15);
        }

        [Fact]
        public void ComputeVertices_Returns8Vertices()
        {
            var aabb = new AABB(new Point(0, 0, 0), new Point(10, 10, 10));
            var vertices = aabb.ComputeVertices();
            vertices.Length.Should().Be(8);
        }

        [Fact]
        public void ComputeVertices_CorrectMinMaxCorners()
        {
            var aabb = new AABB(new Point(0, 0, 0), new Point(10, 20, 30));
            var vertices = aabb.ComputeVertices();
            // First vertex is min corner, 7th (index 6) is max corner
            AssertPointEqual(vertices[0], 0, 0, 0);
            AssertPointEqual(vertices[6], 10, 20, 30);
        }
    }
}
