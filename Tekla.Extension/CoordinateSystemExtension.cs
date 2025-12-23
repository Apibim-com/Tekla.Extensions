using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;

namespace Tekla.Extension
{
    /// <summary>
    /// Class for working with <see cref="CoordinateSystem"/>
    /// </summary>
    public static class CoordinateSystemExtension
    {
        /// <summary>
        /// Gets the Z-axis vector of the coordinate system by calculating the cross product of X and Y axes.
        /// </summary>
        /// <param name="cs">The coordinate system.</param>
        /// <returns>The Z-axis vector.</returns>
        public static Vector GetAxisZ(this CoordinateSystem cs)
        {
            return cs.AxisX.Cross(cs.AxisY);
        }

        /// <summary>
        /// Converts a coordinate system to a transformation plane.
        /// </summary>
        /// <param name="cs">The coordinate system to convert.</param>
        /// <returns>A transformation plane based on the coordinate system.</returns>
        public static TransformationPlane ToTransformationPlane(this CoordinateSystem cs)
        {
            return new TransformationPlane(cs);
        }

        /// <summary>
        /// Creates a transformation matrix from local to global coordinates.
        /// </summary>
        /// <param name="cs">The coordinate system.</param>
        /// <returns>A matrix for transforming from local to global coordinates.</returns>
        public static Matrix MatrixToGlobal(this CoordinateSystem cs)
        {
            return MatrixFactory.FromCoordinateSystem(cs);
        }

        /// <summary>
        /// Creates a transformation matrix from global to local coordinates.
        /// </summary>
        /// <param name="cs">The coordinate system.</param>
        /// <returns>A matrix for transforming from global to local coordinates.</returns>
        public static Matrix MatrixToLocal(this CoordinateSystem cs)
        {
            return MatrixFactory.ToCoordinateSystem(cs);
        }

        /// <summary>
        /// Converts a coordinate system to a geometric plane.
        /// </summary>
        /// <param name="cs">The coordinate system to convert.</param>
        /// <returns>A geometric plane based on the coordinate system.</returns>
        public static GeometricPlane ToGeometricPlane(this CoordinateSystem cs)
        {
            return new GeometricPlane(cs);
        }
    }
}
