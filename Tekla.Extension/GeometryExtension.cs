using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tekla.Structures;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;

namespace Tekla.Extension;
/// <summary>
/// Class for working with 3d geometry
/// </summary>
public static class GeometryExtension
{
    /// <summary>
    /// Determines whether a test point lies on a line segment within a tolerance.
    /// </summary>
    /// <param name="lineSegment">The line segment to test against.</param>
    /// <param name="testPoint">The point to test.</param>
    /// <param name="tolerance">The distance tolerance (default is 0.01).</param>
    /// <returns>True if the point is on the line segment within tolerance; otherwise, false.</returns>
    public static bool IsPointInLineSegment3d(LineSegment lineSegment, Point testPoint, double tolerance = 0.01)
    {
        return IsPointInLineSegment3d(lineSegment.Point1, lineSegment.Point2, testPoint, tolerance);
    }

    /// <summary>
    /// Determines whether a test point lies on a line segment defined by start and end points within a tolerance.
    /// </summary>
    /// <param name="startPoint">The starting point of the line segment.</param>
    /// <param name="endPoint">The ending point of the line segment.</param>
    /// <param name="testPoint">The point to test.</param>
    /// <param name="tolerance">The distance tolerance (default is 0.01).</param>
    /// <returns>True if the point is on the line segment within tolerance; otherwise, false.</returns>
    public static bool IsPointInLineSegment3d(Point startPoint, Point endPoint, Point testPoint, double tolerance = 0.01)
    {
        Vector lineVector = new Vector(endPoint - startPoint);
        Vector testPointVector = new Vector(testPoint - startPoint);

        double dotProduct = Math.Round(lineVector.Dot(testPointVector), 2);
        double magnitude = Math.Round(lineVector.GetLengthSquared(), 2);

        if (dotProduct < 0 || dotProduct > magnitude)
            return false;
        else
        {
            double distance = testPointVector.Cross(lineVector).GetLength() / lineVector.GetLength();
            return distance <= tolerance;
        }
    }

    /// <summary>
    /// Determines whether a point is inside a polygon using the winding number algorithm.
    /// </summary>
    /// <param name="testPoint">The point to test.</param>
    /// <param name="polygon">The polygon defined by a collection of points.</param>
    /// <param name="includeLine">Whether to include points on the polygon boundary (default is true).</param>
    /// <returns>True if the point is inside the polygon; otherwise, false.</returns>
    public static bool IsPointInsidePolygon(this Point testPoint, IReadOnlyCollection<Point> polygon, bool includeLine = true)
    {
        int windingNumber = 0;
        for (int i = 0; i < polygon.Count; i++)
        {
            int j = i == 0 ? polygon.Count - 1 : i - 1;
            Point point1 = polygon.ElementAt(i);
            Point point2 = polygon.ElementAt(j);

            if (point1.Y <= testPoint.Y)
            {
                if (point2.Y > testPoint.Y && IsPointLeftOnEdge(point1, point2, testPoint))
                {
                    windingNumber++;
                }
            }
            else
            {
                if (point2.Y <= testPoint.Y && IsPointLeftOnEdge(point2, point1, testPoint))
                {
                    windingNumber--;
                }
            }


            if (includeLine)
            {
                if (IsPointInLineSegment3d(point1, point2, testPoint))
                    return true;
            }
        }
        return windingNumber != 0;
    }

    private static bool IsPointLeftOnEdge(Point point1, Point point2, Point testPoint)
    {
        return ((point2.X - point1.X) * (testPoint.Y - point1.Y)) - ((testPoint.X - point1.X) * (point2.Y - point1.Y)) > 0;
    }
}
