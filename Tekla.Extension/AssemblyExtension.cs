using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tekla.Structures;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using SR = System.Reflection;

namespace Tekla.Extension
{
    /// <summary>
    /// Class for working with <see cref="Assembly"/> in Tekla Structures
    /// </summary>
    public static class AssemblyExtension
    {
        /// <summary>
        /// Calculates the axis-aligned bounding box for an assembly by combining all parts' bounding boxes.
        /// </summary>
        /// <param name="assembly">The assembly to calculate the bounding box for.</param>
        /// <returns>The AABB that encompasses the entire assembly.</returns>
        public static AABB GetBoundingBox(this Assembly assembly)
        {
            Point min = PointExtension.MinPoint;
            Point max = PointExtension.MaxPoint;

            var parts = assembly.GetAllPartsOfAssembly().Cast<Part>().ToList();

            foreach (Part item in parts)
            {
                Solid solid = item.GetSolid();
                Point min_point = solid.MinimumPoint;
                Point max_point = solid.MaximumPoint;

                PointExtension.ComparePoints(min_point, min, (x1, x2) => x1 > x2);
                PointExtension.ComparePoints(max_point, min, (x1, x2) => x1 > x2);
                PointExtension.ComparePoints(min_point, max, (x1, x2) => x1 < x2);
                PointExtension.ComparePoints(max_point, max, (x1, x2) => x1 < x2);
            }

            return new AABB(min, max);
        }

        /// <summary>
        /// Gets all parts in an assembly, optionally including parts from sub-assemblies.
        /// </summary>
        /// <param name="assembly">The assembly to get parts from.</param>
        /// <param name="isIncludeSubAssemblies">Whether to include parts from sub-assemblies (default is false).</param>
        /// <returns>A read-only collection of all parts in the assembly.</returns>
        public static IReadOnlyCollection<Part> GetAllPartsOfAssembly(this Assembly assembly, bool isIncludeSubAssemblies = false)
        {
            ICollection<Assembly> subAssemblies = isIncludeSubAssemblies ? GetSubAssemblies(assembly) : (new Assembly[] { assembly });
            List<Part> parts = new();

            foreach (Assembly item in subAssemblies)
            {
                parts.Add(item.GetMainPart() as Part);
                parts.AddRange(item.GetSecondaries().Cast<Part>());
            }

            return parts;
        }
        private static ICollection<Assembly> GetSubAssemblies(Assembly assembly)
        {
            ArrayList subAssemblies = assembly.GetSubAssemblies();

            if (subAssemblies.Count == 0)
            {
                return new Assembly[] { assembly };
            }
            else
            {
                List<Assembly> assemblies = new();
                foreach (Assembly item in subAssemblies)
                {
                    assemblies.AddRange(GetSubAssemblies(item));
                }
                return assemblies;
            }
        }

        /// <summary>
        /// Calculates the oriented bounding box for an assembly by combining all parts' OBBs.
        /// </summary>
        /// <param name="assembly">The assembly to calculate the OBB for.</param>
        /// <returns>The OBB that encompasses the entire assembly.</returns>
        public static OBB GetOBB(this Assembly assembly)
        {
            IEnumerable<OBB> obbs = assembly.GetAllPartsOfAssembly().Select(p => p.GetPartOBB());
            return OBBExtension.CombineOBBs(obbs);
        }

        /// <summary>
        /// Gets the assembly drawing associated with this assembly.
        /// </summary>
        /// <param name="assembly">The assembly to get the drawing for.</param>
        /// <returns>The assembly drawing if it exists; otherwise, null.</returns>
        public static Tekla.Structures.Drawing.AssemblyDrawing GetAssemblyDrawing(this Assembly assembly)
        {
            int id = assembly.GetReportProperty<int>("DRAWING.ID");
            if (id > 0)
            {
                Identifier thisIdentifier = new(id);
                Tekla.Structures.Drawing.AssemblyDrawing assemblyDrawing = new(assembly.Identifier);
                assemblyDrawing.GetType()
                    .GetProperty("Identifier", SR.BindingFlags.Instance | SR.BindingFlags.Public | SR.BindingFlags.NonPublic)
                    .SetValue(assemblyDrawing, thisIdentifier);
                _ = assemblyDrawing.Select();
                return assemblyDrawing;
            }
            return null;
        }

        /// <summary>
        /// Finds all assemblies that are spatially around this assembly (within its bounding box).
        /// </summary>
        /// <param name="assembly">The assembly to search around.</param>
        /// <param name="model">The model to search in (uses active model if null).</param>
        /// <returns>A read-only collection of nearby assemblies.</returns>
        public static IReadOnlyCollection<Assembly> FindAssembliesAround(this Assembly assembly, Model model = null)
        {
            model ??= new Model();

            OBB obb = assembly.GetOBB();

            IEnumerable<Part> parts = model.GetModelObjectSelector()
                .GetObjectsByBoundingBox(obb.GetMinimumPoint(), obb.GetMaximumPoint())
                .ToIEnumerable<Part>();

            Dictionary<Guid, Assembly> neighbourAssemblies = new();
            foreach (Part part in parts)
            {
                Assembly thisAssembly = part.GetAssembly();

                if (thisAssembly.Identifier == assembly.Identifier)
                {
                    continue;
                }

                if (!neighbourAssemblies.ContainsKey(thisAssembly.Identifier.GUID))
                {
                    neighbourAssemblies.Add(thisAssembly.Identifier.GUID, thisAssembly);
                }
            }

            return neighbourAssemblies.Values;
        }
    }
}
