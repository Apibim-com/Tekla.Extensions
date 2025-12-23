using System.Collections.Generic;
using System.Linq;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;

namespace Tekla.Extension
{
    /// <summary>
    /// Class for working with <see cref="BoltGroup"/>
    /// </summary>
    public static class BoltExtension
    {
        /// <summary>
        /// Gets all parts connected by the bolt group including main part, part to bolt to, and other parts.
        /// </summary>
        /// <param name="boltGroup">The bolt group to get parts from.</param>
        /// <returns>A read-only collection of all parts connected by the bolt group.</returns>
        public static IReadOnlyCollection<Part> GetAllParts(this BoltGroup boltGroup)
        {
            List<Part> parts = new List<Part>();
            if (boltGroup.PartToBoltTo is not null)
                parts.Add(boltGroup.PartToBoltTo);
            if (boltGroup.PartToBeBolted is not null)
                parts.Add(boltGroup.PartToBeBolted);
            if (boltGroup.OtherPartsToBolt.Count > 0)
                parts.AddRange(boltGroup.OtherPartsToBolt.Cast<Part>().Where(p => p is not null));
            return parts;
        }

        /// <summary>
        /// Gets all bolt positions from the bolt group.
        /// </summary>
        /// <param name="boltGroup">The bolt group to get positions from.</param>
        /// <returns>A read-only collection of bolt positions as points.</returns>
        public static IReadOnlyCollection<Point> GetBoltPoints(this BoltGroup boltGroup)
        {
            return boltGroup.BoltPositions.Cast<Point>().ToArray();
        }

        /// <summary>
        /// Sets all parts to be bolted. First part becomes PartToBeBolted, second becomes PartToBoltTo, and remaining parts are added to OtherPartsToBolt.
        /// </summary>
        /// <param name="boltGroup">The bolt group to set parts for.</param>
        /// <param name="parts">The collection of parts to bolt together.</param>
        public static void SetAllPartToBolt(this BoltGroup boltGroup, IEnumerable<Part> parts)
        {
            int i = 0;
            foreach (Part part in parts)
            {
                if (i == 0 && part is not null)
                    boltGroup.PartToBeBolted = part;
                else if (i == 1 && part is not null)
                    boltGroup.PartToBoltTo = part;
                else if (part is not null)
                    boltGroup.OtherPartsToBolt.Add(part);

                i++;
            }
        }

        /// <summary>
        /// Fills the bolt group with all bolt settings and parts information.
        /// </summary>
        /// <param name="boltGroup">The bolt group to configure.</param>
        /// <param name="boltSettings">The bolt settings including type, size, standard, and configuration.</param>
        /// <param name="partToBeBolted">The primary part to be bolted.</param>
        /// <param name="partToBoltTo">The part to bolt to (optional).</param>
        /// <param name="otherParts">Additional parts to include in the bolt group (optional).</param>
        public static void FillFullInfoToBoltGroup(this BoltGroup boltGroup, BoltSettings boltSettings, Part partToBeBolted, Part partToBoltTo = null, IEnumerable<Part> otherParts = null)
        {
            boltGroup.PartToBeBolted = partToBoltTo;
            if (partToBeBolted is not null)
                boltGroup.PartToBoltTo = partToBoltTo;
            if (otherParts is not null)
            {
                foreach (Part part in otherParts)
                    if (part is not null)
                        boltGroup.AddOtherPartToBolt(part);
            }

            if (boltSettings is null)
                return;

            boltGroup.BoltStandard = boltSettings.BoltStandard;
            boltGroup.BoltSize = boltSettings.BoltSize;
            boltGroup.BoltType = boltSettings.BoltType;
            boltGroup.ConnectAssemblies = boltSettings.ConnectAssemblies;
            boltGroup.CutLength = boltSettings.CutLength;
            boltGroup.Tolerance = boltSettings.Tolerance;
            boltGroup.ExtraLength = boltSettings.ExtraLength;

            if (boltSettings.BoltConfiguration.Length == 6)
            {
                boltGroup.Bolt = boltSettings.BoltConfiguration[0];
                boltGroup.Nut1 = boltSettings.BoltConfiguration[1];
                boltGroup.Nut2 = boltSettings.BoltConfiguration[2];
                boltGroup.Washer1 = boltSettings.BoltConfiguration[3];
                boltGroup.Washer2 = boltSettings.BoltConfiguration[4];
                boltGroup.Washer3 = boltSettings.BoltConfiguration[5];
            }
        }

        /// <summary>
        /// Configuration settings for bolt groups.
        /// </summary>
        public class BoltSettings
        {
            /// <summary>
            /// Gets or sets the bolt type.
            /// </summary>
            public BoltGroup.BoltTypeEnum BoltType { get; set; }

            /// <summary>
            /// Gets or sets the bolt size (diameter).
            /// </summary>
            public double BoltSize { get; set; }

            /// <summary>
            /// Gets or sets the bolt standard specification.
            /// </summary>
            public string BoltStandard { get; set; }

            /// <summary>
            /// Gets or sets whether to connect assemblies.
            /// </summary>
            public bool ConnectAssemblies { get; set; }

            /// <summary>
            /// Gets or sets the extra length for the bolt.
            /// </summary>
            public double ExtraLength { get; set; }

            /// <summary>
            /// Gets or sets the cut length for the bolt.
            /// </summary>
            public double CutLength { get; set; }

            /// <summary>
            /// Gets or sets the tolerance for bolt positioning.
            /// </summary>
            public double Tolerance { get; set; }

            /// <summary>
            /// Gets or sets the bolt configuration array: [Bolt, Nut1, Nut2, Washer1, Washer2, Washer3].
            /// </summary>
            public bool[] BoltConfiguration { get; set; }
        }

        /// <summary>
        /// Sets the first and second positions of the bolt group from a collection of points.
        /// </summary>
        /// <param name="boltGroup">The bolt group to set positions for.</param>
        /// <param name="points">The collection of points (first and last points are used).</param>
        public static void SetPointsPositionToBolt(this BoltGroup boltGroup, IEnumerable<Point> points)
        {
            boltGroup.FirstPosition = points.FirstOrDefault();
            boltGroup.SecondPosition = points.LastOrDefault();
        }

        /// <summary>
        /// Sets the start point offset in the X direction for the bolt group.
        /// </summary>
        /// <param name="boltGroup">The bolt group to set offset for.</param>
        /// <param name="distance">The offset distance in the X direction.</param>
        public static void SetStartPointDxOffset(this BoltGroup boltGroup, double distance)
        {
            boltGroup.StartPointOffset = new Offset() { Dx = distance, Dy = 0, Dz = 0 };
        }

        /// <summary>
        /// Calculates the sum of all bolt distances in the X direction.
        /// </summary>
        /// <param name="boltArray">The bolt array to calculate from.</param>
        /// <returns>The total distance in the X direction.</returns>
        public static double GetSumDistX(this BoltArray boltArray)
        {
            int boltDistXCount = boltArray.GetBoltDistXCount();
            double num = 0.0;
            for (int i = 0; i < boltDistXCount; i++)
            {
                num += boltArray.GetBoltDistX(i);
            }
            return num;
        }

        /// <summary>
        /// Calculates the sum of all bolt distances in the Y direction.
        /// </summary>
        /// <param name="boltArray">The bolt array to calculate from.</param>
        /// <returns>The total distance in the Y direction.</returns>
        public static double GetSumDistY(this BoltArray boltArray)
        {
            int boltDistYCount = boltArray.GetBoltDistYCount();
            double num = 0.0;
            for (int i = 0; i < boltDistYCount; i++)
            {
                num += boltArray.GetBoltDistY(i);
            }
            return num;
        }
    }
}
