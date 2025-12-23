using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;

namespace Tekla.Extension;
/// <summary>
/// Class for working with <see cref="Beam"/> in Tekla Structures
/// </summary>
public static class BeamExtension
{
    /// <summary>
    /// Gets a line segment representing the beam from start point to end point.
    /// </summary>
    /// <param name="beam">The beam to get the line segment from.</param>
    /// <returns>A line segment from the beam's start point to end point.</returns>
    public static LineSegment GetLineSegment(this Beam beam)
    {
        return new LineSegment(beam.StartPoint, beam.EndPoint);
    }

    /// <summary>
    /// Gets the center line segment of the beam, optionally including cuts and fittings.
    /// </summary>
    /// <param name="beam">The beam to get the center line from.</param>
    /// <param name="withCutsFittings">Whether to include cuts and fittings in the calculation (default is true).</param>
    /// <returns>The center line segment of the beam.</returns>
    public static LineSegment GetCenterLineSegment(this Beam beam, bool withCutsFittings = true)
    {
        ArrayList centerLine = beam.GetCenterLine(withCutsFittings);
        if (centerLine.Count == 0)
            return new LineSegment();
        Point point1 = centerLine[0] as Point;
        Point point2 = centerLine[1] as Point;
        return new LineSegment(point1, point2);
    }

    /// <summary>
    /// Gets the center point of the beam's center line.
    /// </summary>
    /// <param name="beam">The beam to get the center point from.</param>
    /// <param name="withCutsFittings">Whether to include cuts and fittings in the calculation (default is false).</param>
    /// <returns>The center point of the beam.</returns>
    public static Point GetCenterPoint(this Beam beam, bool withCutsFittings = false)
    {
        return GetCenterLineSegment(beam).GetCenterPoint();
    }

    /// <summary>
    /// Gets the direction vector of the beam from start point to end point.
    /// </summary>
    /// <param name="beam">The beam to get the direction vector from.</param>
    /// <returns>The direction vector of the beam.</returns>
    public static Vector GetVector(this Beam beam)
    {
        return new Vector(beam.EndPoint - beam.StartPoint);
    }

    /// <summary>
    /// Calculates the length of the beam from start point to end point.
    /// </summary>
    /// <param name="beam">The beam to calculate the length of.</param>
    /// <returns>The length of the beam.</returns>
    public static double GetBeamLength(this Beam beam)
    {
        return Distance.PointToPoint(beam.StartPoint, beam.EndPoint);
    }
}
