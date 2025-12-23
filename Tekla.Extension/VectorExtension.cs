using System;
using Tekla.Structures.Geometry3d;

namespace Tekla.Extension
{
    /// <summary>
    /// Extension class for working with <see cref="Vector"/> in Tekla Structures
    /// </summary>
    public static class VectorExtension
    {
        #region Properties
        /// <summary>
        /// Global X
        /// </summary>
        public static Vector X => new(1, 0, 0);
        /// <summary>
        /// Global Y
        /// </summary>
        public static Vector Y => new(0, 1, 0);
        /// <summary>
        /// Global Z
        /// </summary>
        public static Vector Z => new(0, 0, 1);
        /// <summary>
        /// Null vector
        /// </summary>
        public static Vector Null => new(0, 0, 0);
        /// <summary>
        /// Nan vector
        /// </summary>
        public static Vector Nan => new(double.NaN, double.NaN, double.NaN);
        #endregion

        #region Methods
        /// <summary>
        /// Summing vector to vector
        /// </summary>
        /// <param name="vector">First vector</param>
        /// <param name="vectorToAdd">Second vector</param>
        /// <returns>Result vector</returns>
        public static Vector Add(this Vector vector, Point vectorToAdd)
        {
            return new Vector(vector.ToPoint() + vectorToAdd);
        }
        /// <summary>
        /// Substracting vector to vector
        /// </summary>
        /// <param name="vector">First vector</param>
        /// <param name="vectorToAdd">Second vector</param>
        /// <returns>Result vector</returns>
        public static Vector Subtract(this Vector vector, Point vectorToAdd)
        {
            return new Vector(vector.ToPoint() - vectorToAdd);
        }
        /// <summary>
        /// Transform vector to point
        /// </summary>
        /// <param name="vector">Vector to be transformed</param>
        /// <returns>Transformed point</returns>
        public static Point ToPoint(this Vector vector)
        {
            return new Point(vector);
        }
        /// <summary>
        /// Negative vector
        /// </summary>
        /// <param name="vector">Origin vector</param>
        /// <returns>Negative vector</returns>
        public static Vector Negative(this Vector vector)
        {
            return new Vector(-vector.X, -vector.Y, -vector.Z);
        }
        /// <summary>
        /// Projects points according given vector
        /// </summary>
        /// <param name="point">Point to be projected</param>
        /// <param name="vector">Vector for calculation projection</param>
        /// <returns>Point result</returns>
        public static Point ProjectPointToVector(this Point point, Vector vector)
        {
            double dotProduct = (point.X * vector.X) + (point.Y * vector.Y) + (point.Z * vector.Z);
            double magnitude = Math.Sqrt((vector.X * vector.X) + (vector.Y * vector.Y) + (vector.Z * vector.Z));
            double projectionScalar = dotProduct / (magnitude * magnitude);
            return new Point(projectionScalar * vector.X, projectionScalar * vector.Y, projectionScalar * vector.Z);
        }
        /// <summary>
        /// Rounds the coordinates of vector
        /// </summary>
        /// <param name="vector">Vector to be rounded</param>
        /// <param name="digits">Digits</param>
        /// <returns>Rounded vector</returns>
        public static Vector Round(this Vector vector, int digits = 2)
        {
            vector.X = Math.Round(vector.X, digits);
            vector.Y = Math.Round(vector.Y, digits);
            vector.Z = Math.Round(vector.Z, digits);
            return vector;
        }
        /// <summary>
        /// Transform vector according matrix
        /// </summary>
        /// <param name="matrix">Transormation matrix</param>
        /// <param name="vector">Vector to be transformed</param>
        /// <returns>Result vector</returns>
        public static Vector TransformVector(this Matrix matrix, Vector vector)
        {
            Vector point0 = new();
            Point p0Transformed = matrix.Transform(point0);
            Point vectorPointTransformed = matrix.Transform(vector);
            return new(vectorPointTransformed - p0Transformed);
        }

        /// <summary>
        /// Translates a point by a vector using the vector's X, Y, and Z components.
        /// </summary>
        /// <param name="point">The point to translate.</param>
        /// <param name="vector">The vector defining the translation direction and magnitude.</param>
        public static void Translate(this Point point, Vector vector)
        {
            point.Translate(vector.X, vector.Y, vector.Z);
        }
        /// <summary>
        /// Gets the squared length (magnitude) of a vector without calculating the square root.
        /// Useful for performance-critical comparisons where actual length is not needed.
        /// </summary>
        /// <param name="vector">The vector to calculate the squared length for.</param>
        /// <returns>The squared length of the vector.</returns>
        public static double GetLengthSquared(this Vector vector)
        {
            return vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z;
        }
        #endregion
    }
}
