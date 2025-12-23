using System.Collections.Generic;
using System.Linq;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using Tekla.Structures.Model.UI;

namespace Tekla.Extension.UI
{
    /// <summary>
    /// Class for working with <see cref="Picker"/> in Tekla Structures
    /// </summary>
    public static class PickerModelExtension
    {
        /// <summary>
        /// Picks model objects from the Tekla Structures UI and returns them as a strongly-typed enumerable.
        /// </summary>
        /// <typeparam name="T">The type of model objects to pick (must inherit from ModelObject).</typeparam>
        /// <param name="picker">The Picker instance.</param>
        /// <param name="_enum">The picking mode (single or multiple objects).</param>
        /// <param name="prompt">Optional prompt message to display to the user. Default is empty string.</param>
        /// <returns>An enumerable collection of picked objects of type T.</returns>
        public static IEnumerable<T> PickObjects<T>(this Picker picker, Picker.PickObjectsEnum _enum, string prompt = "") where T : ModelObject
        {
            return picker.PickObjects(_enum, prompt).ToIEnumerable<T>();
        }
        /// <summary>
        /// Picks points from the Tekla Structures UI and returns them as an enumerable collection.
        /// </summary>
        /// <param name="picker">The Picker instance.</param>
        /// <param name="_enum">The picking mode (single or multiple points).</param>
        /// <param name="prompt">Optional prompt message to display to the user. Default is empty string.</param>
        /// <returns>An enumerable collection of picked points.</returns>
        public static IEnumerable<Point> PickPointsEnumerable(this Picker picker, Picker.PickPointEnum _enum, string prompt = "")
        {
            return picker.PickPoints(_enum, prompt).Cast<Point>();
        }
    }
}
