using System;
using System.Collections.Generic;
using System.Linq;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;

namespace Tekla.Extension
{
    /// <summary>
    /// Extension class for working with <see cref="Point"/> in Tekla Structures
    /// </summary>
    public static class PointExtension
    {
        /// <summary>
        /// Gets a point with maximum possible coordinate values.
        /// </summary>
        public static Point MaxPoint => new Point(double.MaxValue, double.MaxValue, double.MaxValue);
        /// <summary>
        /// Gets a point with minimum possible coordinate values.
        /// </summary>
        public static Point MinPoint => new Point(double.MinValue, double.MinValue, double.MinValue);
        /// <summary>
        /// Returns the center point between two points
        /// </summary>
        /// <param name="point1">Point 1</param>
        /// <param name="point2">Point 2</param>
        /// <returns>Center point</returns>
        public static Point GetCenterPoint(this Point point1, Point point2)
        {
            return point1 + (GetVector(point1, point2) * 0.5);
        }

        /// <summary>
        /// Returns the vector of two points
        /// </summary>
        /// <param name="startPoint">Start point</param>
        /// <param name="endPoint">End point</param>
        /// <returns>Vector of two points</returns>
        public static Vector GetVector(Point startPoint, Point endPoint)
        {
            return new Vector(endPoint - startPoint);
        }
        /// <summary>
        /// Rounds the coordinates of point
        /// </summary>
        /// <param name="point">Point to be rounded</param>
        /// <param name="digits">Digits</param>
        /// <returns>Rounded point</returns>
        public static Point Round(this Point point, int digits = 2)
        {
            point.X = Math.Round(point.X, digits);
            point.Y = Math.Round(point.Y, digits);
            point.Z = Math.Round(point.Z, digits);
            return point;
        }
        /// <summary>
        /// Get the nearest point of origin point
        /// </summary>
        /// <param name="originPoint">The point to compare</param>
        /// <param name="point1">First point</param>
        /// <param name="point2">Second point</param>
        /// <returns>The nearest point (first or second)</returns>
        public static Point GetNearestPoint(this Point originPoint, Point point1, Point point2)
        {
            return (Distance.PointToPoint(originPoint, point1) < Distance.PointToPoint(originPoint, point2)) ? point1 : point2;
        }
        /// <summary>
        /// Get the nearest point of collection of points
        /// </summary>
        /// <param name="originPoint">The point to compare</param>
        /// <param name="points">Array of points</param>
        /// <returns>The nearest point in array</returns>
        public static Point GetNearestPoint(this Point originPoint, IEnumerable<Point> points)
        {
            return points.OrderBy(p => Distance.PointToPoint(originPoint, p)).FirstOrDefault();
        }
        /// <summary>
        /// Get the remote point of collection of points
        /// </summary>
        /// <param name="originPoint">The point to compare</param>
        /// <param name="points">Array of points</param>
        /// <returns>The remote point in array</returns>
        public static Point GetRemotePoint(this Point originPoint, IEnumerable<Point> points)
        {
            return points.OrderByDescending(p => Distance.PointToPoint(originPoint, p)).FirstOrDefault();
        }
        /// <summary>
        /// Convert Point to ContourPoint
        /// </summary>
        /// <param name="point">The point to convert</param>
        /// <param name="chamfer">The chamber if need</param>
        /// <returns>The ContourPoint</returns>
        public static ContourPoint ToContourPoint(this Point point, Chamfer chamfer = null)
        {
            return new ContourPoint(point, chamfer);
        }
        /// <summary>
        /// Convert ContourPoint to Point
        /// </summary>
        /// <param name="contourPoint">The contour point to convert</param>
        /// <returns>The point</returns>
        public static Point ToPoint(this ContourPoint contourPoint)
        {
            return new Point(contourPoint.X, contourPoint.Y, contourPoint.Z);
        }
        /// <summary>
        /// Get the minimun distance in collection of points
        /// </summary>
        /// <param name="points"></param>
        /// <returns>Minimum value</returns>
        public static double GetMinimumDistance(this IEnumerable<Point> points)
        {
            return points.Select((p, i) => SelectDistances(points, p, i))
                .Min();
        }
        /// <summary>
        /// Get the maximum distance in collection of points
        /// </summary>
        /// <param name="points"></param>
        /// <returns>Maximum value</returns>
        public static double GetMaximumDistance(this IEnumerable<Point> points)
        {
            return points.Select((p, i) => SelectDistances(points, p, i))
                .Max();
        }

        private static double SelectDistances(IEnumerable<Point> points, Point point, int index)
        {
            int k = index == 0 ? points.Count() - 1 : index - 1;
            return Distance.PointToPoint(points.ElementAt(k), point);
        }
        /// <summary>
        /// Convert point to vector
        /// </summary>
        /// <param name="point"></param>
        /// <returns>Vector</returns>
        public static Vector ToVector(this Point point)
        {
            return new Vector(point);
        }
        /// <summary>
        /// Check if point is null
        /// </summary>
        /// <param name="point"></param>
        /// <returns>Is point is null</returns>
        public static bool IsNull(this Point point)
        {
            return point.X == double.NaN || point.Y == double.NaN || point.Z == double.NaN;
        }
        /// <summary>
        /// Check if point is empty
        /// </summary>
        /// <param name="point"></param>
        /// <returns>Is point is null</returns>
        public static bool IsEmpty(this Point point)
        {
            return point.X == 0 && point.Y == 0 && point.Z == 0;
        }

        /// <summary>
        /// Get collection of line segments of polygon
        /// </summary>
        /// <param name="points">Points resresenting polygon</param>
        /// <param name="isClosed">Is polygon closed</param>
        /// <returns></returns>
        public static IReadOnlyCollection<LineSegment> GetLineSegmentsOfPolygon(this IEnumerable<Point> points, bool isClosed = true)
        {
            LineSegment[] lineSegments = new LineSegment[isClosed ? points.Count() : points.Count() - 1];

            for (int i = 0; i < lineSegments.Length; i++)
            {
                if (i == lineSegments.Length - 1 && isClosed)
                    lineSegments[i] = new LineSegment(points.Last(), points.First());
                else
                    lineSegments[i] = new LineSegment(points.ElementAt(i), points.ElementAt(i + 1));
            }
            return lineSegments;
        }

        /// <summary>
        /// Compares two points coordinate by coordinate and writes the result to pointToWrite based on the comparison function.
        /// </summary>
        /// <param name="thisPoint">The point to compare from.</param>
        /// <param name="pointToWrite">The point to write comparison results to.</param>
        /// <param name="Comparer">Optional comparison function. Default returns the maximum of two values.</param>
        public static void ComparePoints(Point thisPoint, Point pointToWrite, Func<double, double, bool> Comparer = null)
        {
            Comparer ??= (x, y) => Math.Max(x, y) == x;

            if (Comparer(thisPoint.X, pointToWrite.X))
                pointToWrite.X = thisPoint.X;
            if (Comparer(thisPoint.Y, pointToWrite.Y))
                pointToWrite.Y = thisPoint.Y;
            if (Comparer(thisPoint.Z, pointToWrite.Z))
                pointToWrite.Z = thisPoint.Z;
        }
        /// <summary>
        /// Gets the Axis-Aligned Bounding Box (AABB) for a collection of points representing a polygon.
        /// </summary>
        /// <param name="points">Collection of points representing the polygon.</param>
        /// <returns>An AABB that encompasses all the points.</returns>
        public static AABB GetPolygonAABB(IEnumerable<Point> points)
        {
            Point min = MaxPoint;
            Point max = MinPoint;

            foreach (Point item in points)
            {
                ComparePoints(item, max, (x, y) => x > y);
                ComparePoints(item, min, (x, y) => x < y);
            }

            return new AABB(min, max);
        }

        /// <summary>
        /// Rounds each coordinate of a point to the nearest multiple of the specified number.
        /// </summary>
        /// <param name="point">The point to round.</param>
        /// <param name="num">The number to round to the nearest multiple of.</param>
        /// <returns>A point with rounded coordinates.</returns>
        public static Point RoundTo(this Point point, int num)
        {
            point.X = Math.Round(point.X / num, 0) * num;
            point.Y = Math.Round(point.Y / num, 0) * num;
            point.Z = Math.Round(point.Z / num, 0) * num;
            return point;
        }
        /// <summary>
        /// Rounds each coordinate of a point up to the nearest multiple of the specified number.
        /// </summary>
        /// <param name="point">The point to ceiling round.</param>
        /// <param name="num">The number to round up to the nearest multiple of.</param>
        /// <returns>A point with ceiling rounded coordinates.</returns>
        public static Point CeilingTo(this Point point, int num)
        {
            point.X = Math.Ceiling(point.X / num) * num;
            point.Y = Math.Ceiling(point.Y / num) * num;
            point.Z = Math.Ceiling(point.Z / num) * num;
            return point;
        }
        /// <summary>
        /// Rounds each coordinate of a point down to the nearest multiple of the specified number.
        /// </summary>
        /// <param name="point">The point to floor round.</param>
        /// <param name="num">The number to round down to the nearest multiple of.</param>
        /// <returns>A point with floor rounded coordinates.</returns>
        public static Point FloorTo(this Point point, int num)
        {
            point.X = Math.Floor(point.X / num) * num;
            point.Y = Math.Floor(point.Y / num) * num;
            point.Z = Math.Floor(point.Z / num) * num;
            return point;
        }
        /// <summary>
        /// Creates a new point with the X coordinate set to the specified value while keeping Y and Z unchanged.
        /// </summary>
        /// <param name="point">The original point.</param>
        /// <param name="value">The new X coordinate value.</param>
        /// <returns>A new point with the updated X coordinate.</returns>
        public static Point ResetX(this Point point, double value)
        {
            return new Point(value, point.Y, point.Z);
        }
        /// <summary>
        /// Creates a new point with the Y coordinate set to the specified value while keeping X and Z unchanged.
        /// </summary>
        /// <param name="point">The original point.</param>
        /// <param name="value">The new Y coordinate value.</param>
        /// <returns>A new point with the updated Y coordinate.</returns>
        public static Point ResetY(this Point point, double value)
        {
            return new Point(point.X, value, point.Z);
        }
        /// <summary>
        /// Creates a new point with the Z coordinate set to the specified value while keeping X and Y unchanged.
        /// </summary>
        /// <param name="point">The original point.</param>
        /// <param name="value">The new Z coordinate value.</param>
        /// <returns>A new point with the updated Z coordinate.</returns>
        public static Point ResetZ(this Point point, double value)
        {
            return new Point(point.X, point.Y, value);
        }
    }
}
