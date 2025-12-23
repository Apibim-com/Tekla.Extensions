using System.Collections.Generic;
using System.Linq;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using Tekla.Structures.Solid;

namespace Tekla.Extension
{
    /// <summary>
    /// Extension methods for working with <see cref="Solid"/> objects in Tekla Structures.
    /// </summary>
    public static class SolidExtension
    {
        /// <summary>
        /// Gets all unique points from a solid by extracting start and end points from all edges.
        /// </summary>
        /// <param name="solid">The solid to extract points from.</param>
        /// <returns>A collection of unique points from the solid's edges.</returns>
        public static ICollection<Point> GetPoints(this Solid solid)
        {
            return solid.GetEdgeEnumerator()
                .ToIEnumerable<Edge>()
                .SelectMany(e => new Point[] { e.StartPoint, e.EndPoint })
                .ToHashSet();

        }
    }
}
