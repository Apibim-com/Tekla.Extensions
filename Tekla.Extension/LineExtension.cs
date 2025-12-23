using System;
using Tekla.Structures.Geometry3d;

namespace Tekla.Extension
{
    /// <summary>
    /// Class for working with <see cref="Line"/>
    /// </summary>
    public static class LineExtension
    {
        /// <summary>
        /// Gets an array of points that represent the arc divided into specified number of steps.
        /// </summary>
        /// <param name="arc">The arc to divide into points.</param>
        /// <param name="steps">Number of steps to divide the arc into. Default is 10.</param>
        /// <returns>An array of points representing the arc.</returns>
        public static Point[] GetPoints(this Arc arc, int steps = 10)
        {
            double angleRadians;
            double stepDegree = arc.Angle / steps;
            Point[] points = new Point[steps + 1];
            for (int i = 0; i <= steps; i++)
            {
                angleRadians = i * stepDegree;
                points[i] = RotatePointAroundAxis(new Vector(arc.StartPoint - arc.CenterPoint), arc.CenterPoint, arc.Normal, angleRadians);
            }
            return points;
        }
        /// <summary>
        /// Rotates a point around an axis using Rodrigues' rotation formula.
        /// </summary>
        /// <param name="startDirection">The starting direction vector from the center point.</param>
        /// <param name="center">The center point of rotation.</param>
        /// <param name="axis">The axis vector to rotate around.</param>
        /// <param name="angleRadians">The angle of rotation in radians.</param>
        /// <returns>The rotated point.</returns>
        public static Point RotatePointAroundAxis(Vector startDirection, Point center, Vector axis, double angleRadians)
        {
            axis.Normalize();
            // Apply Rodrigues' rotation formula
            double cosTheta = Math.Cos(angleRadians);
            double sinTheta = Math.Sin(angleRadians);
            double newX = center.X + (cosTheta + (1 - cosTheta) * axis.X * axis.X) * startDirection.X + ((1 - cosTheta) * axis.X * axis.Y - axis.Z * sinTheta) * startDirection.Y + ((1 - cosTheta) * axis.X * axis.Z + axis.Y * sinTheta) * startDirection.Z;
            double newY = center.Y + ((1 - cosTheta) * axis.Y * axis.X + axis.Z * sinTheta) * startDirection.X + (cosTheta + (1 - cosTheta) * axis.Y * axis.Y) * startDirection.Y + ((1 - cosTheta) * axis.Y * axis.Z - axis.X * sinTheta) * startDirection.Z;
            double newZ = center.Z + ((1 - cosTheta) * axis.Z * axis.X - axis.Y * sinTheta) * startDirection.X + ((1 - cosTheta) * axis.Z * axis.Y + axis.X * sinTheta) * startDirection.Y + (cosTheta + (1 - cosTheta) * axis.Z * axis.Z) * startDirection.Z;

            return new Point(newX, newY, newZ);
        }
        /// <summary>
        /// Divides a line segment into a specified number of equal segments.
        /// </summary>
        /// <param name="segment">The line segment to divide.</param>
        /// <param name="quantity">The number of equal segments to divide into.</param>
        /// <returns>An array of line segments representing the divided segments.</returns>
        public static LineSegment[] DevideBy(this LineSegment segment, int quantity)
        {
            double length = Distance.PointToPoint(segment.Point1, segment.Point2) / quantity;
            Vector stepVector = segment.GetDirectionVector() * length;
            LineSegment[] segments = new LineSegment[quantity];
            for (int i = 0; i < quantity; i++)
            {
                segments[i] = new(segment.Point1 + (stepVector * i), segment.Point1 + (stepVector * (i + 1)));
            }
            return segments;
        }

        /// <summary>
        /// Gets the center point of a line segment.
        /// </summary>
        /// <param name="lineSegment">The line segment to get the center point from.</param>
        /// <returns>The center point of the line segment.</returns>
        public static Point GetCenterPoint(this LineSegment lineSegment)
        {
            return lineSegment.Point1.GetCenterPoint(lineSegment.Point2);
        }

        /// <summary>
        /// Converts a line segment to a line.
        /// </summary>
        /// <param name="lineSegment">The line segment to convert.</param>
        /// <returns>A line created from the line segment.</returns>
        public static Line ToLine(this LineSegment lineSegment)
        {
            return new Line(lineSegment);
        }
    }
}
