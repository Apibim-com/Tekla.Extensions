using System.Collections.Generic;
using Tekla.Structures.Geometry3d;
using TSMUI = Tekla.Structures.Model.UI;

namespace Tekla.Extension
{
    /// <summary>
    /// Class for working with <see cref="AABB"/> in Tekla Structures
    /// </summary>
    public static class AABBExtension
    {
        /// <summary>
        /// Zooms the view to fit the axis-aligned bounding box.
        /// </summary>
        /// <param name="aabb">The AABB to zoom to.</param>
        public static void Zoom(this AABB aabb)
        {
            _ = TSMUI.ViewHandler.ZoomToBoundingBox(aabb);
        }

        /// <summary>
        /// Combines two AABBs into a single AABB that encompasses both.
        /// </summary>
        /// <param name="aabb1">The first AABB.</param>
        /// <param name="aabb2">The second AABB.</param>
        /// <returns>A new AABB that contains both input AABBs.</returns>
        public static AABB Add(this AABB aabb1, AABB aabb2)
        {
            Point minPoint = PointExtension.MinPoint;
            Point maxPoint = PointExtension.MaxPoint;

            PointExtension.ComparePoints(aabb1.MaxPoint, maxPoint, (x1, x2) => x1 < x2);
            PointExtension.ComparePoints(aabb1.MinPoint, minPoint, (x1, x2) => x1 > x2);
            PointExtension.ComparePoints(aabb2.MaxPoint, maxPoint, (x1, x2) => x1 < x2);
            PointExtension.ComparePoints(aabb2.MinPoint, minPoint, (x1, x2) => x1 > x2);

            return new AABB(minPoint, maxPoint);
        }

        /// <summary>
        /// Projects the AABB onto the XY plane, returning the four corner points at Z=0.
        /// </summary>
        /// <param name="aabb">The AABB to project.</param>
        /// <returns>A collection of four points representing the projected AABB corners.</returns>
        public static ICollection<Point> ProjectToXYPlane(this AABB aabb)
        {
            Point point1 = new(aabb.MaxPoint);
            Point point2 = new(aabb.MaxPoint.X, aabb.MinPoint.Y);
            Point point3 = new(aabb.MinPoint);
            Point point4 = new(aabb.MinPoint.X, aabb.MaxPoint.Y);

            point1.Z = point2.Z = point3.Z = point4.Z = 0;
            return new Point[] { point1, point2, point3, point4 };
        }

        /// <summary>
        /// Converts an axis-aligned bounding box to an oriented bounding box.
        /// </summary>
        /// <param name="aabb">The AABB to convert.</param>
        /// <returns>An OBB equivalent to the AABB.</returns>
        public static OBB ToOBB(this AABB aabb)
        {
            Point center = aabb.MinPoint.GetCenterPoint(aabb.MaxPoint);
            Line lineX = new(center, VectorExtension.X);
            Line lineY = new(center, VectorExtension.Y);
            Line lineZ = new(center, VectorExtension.Z);

            Point pointX = Projection.PointToLine(aabb.MaxPoint, lineX);
            Point pointY = Projection.PointToLine(aabb.MaxPoint, lineY);
            Point pointZ = Projection.PointToLine(aabb.MaxPoint, lineZ);

            double[] extents = new double[] { Distance.PointToPoint(center, pointX),
                 Distance.PointToPoint(center, pointY),
                 Distance.PointToPoint(center, pointZ) };

            Vector[] vectors = new Vector[] { VectorExtension.X, VectorExtension.Y, VectorExtension.Z };
            return new OBB(center, vectors, extents);
        }

        /// <summary>
        /// Computes all eight vertices of the AABB.
        /// </summary>
        /// <param name="aabb">The AABB to compute vertices for.</param>
        /// <returns>An array of eight points representing the AABB vertices.</returns>
        public static Point[] ComputeVertices(this AABB aabb)
        {
            return new Point[]
            {
                new Point(aabb.MinPoint.X, aabb.MinPoint.Y, aabb.MinPoint.Z),
                new Point(aabb.MinPoint.X, aabb.MaxPoint.Y, aabb.MinPoint.Z),
                new Point(aabb.MaxPoint.X, aabb.MaxPoint.Y, aabb.MinPoint.Z),
                new Point(aabb.MaxPoint.X, aabb.MinPoint.Y, aabb.MinPoint.Z),

                new Point(aabb.MinPoint.X, aabb.MinPoint.Y, aabb.MaxPoint.Z),
                new Point(aabb.MinPoint.X, aabb.MaxPoint.Y, aabb.MaxPoint.Z),
                new Point(aabb.MaxPoint.X, aabb.MaxPoint.Y, aabb.MaxPoint.Z),
                new Point(aabb.MaxPoint.X, aabb.MinPoint.Y, aabb.MaxPoint.Z),
            };
        }
    }
}
