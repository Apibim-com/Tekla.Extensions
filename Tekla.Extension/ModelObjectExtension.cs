using System;
using System.Collections;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Tekla.Structures.Model;

namespace Tekla.Extension
{
    /// <summary>
    /// Class for working with <see cref="ModelObject"/> in Tekla Structures 
    /// </summary>
    public static class ModelObjectExtension
    {
        /// <summary>
        /// Get universal report property without sensetive case
        /// </summary>
        /// <typeparam name="T">String, Int, Double</typeparam>
        /// <param name="modelObject">Model object to get property</param>
        /// <param name="name">Name of the attribute</param>
        /// <param name="isSuccess">Is found in Tekla Structures</param>
        /// <returns>result from database</returns>
        public static T GetReportProperty<T>(this ModelObject modelObject, string name, out bool isSuccess)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            string correctedName = Regex.Replace(name.ToUpper().Trim(), @"\s{2,}", " ").Replace(" ", "_");
            if (typeof(T) == typeof(string))
            {
                string value = string.Empty;
                isSuccess = modelObject.GetReportProperty(correctedName, ref value);
                return (T)converter.ConvertTo(value, typeof(T));
            }
            else if (typeof(T) == typeof(int))
            {
                int value = int.MinValue;
                isSuccess = modelObject.GetReportProperty(correctedName, ref value);
                return (T)converter.ConvertTo(value, typeof(T));
            }
            else if (typeof(T) == typeof(double))
            {
                double value = double.MinValue;
                isSuccess = modelObject.GetReportProperty(correctedName, ref value);
                return (T)converter.ConvertTo(value, typeof(T));
            }
            else
            {
                isSuccess = false;
                return default;
            }
        }
        /// <summary>
        /// Get universal report property without sensetive case
        /// </summary>
        /// <typeparam name="T">String, Int, Double</typeparam>
        /// <param name="modelObject">Model object to get property</param>
        /// <param name="name">Name of the attribute</param>
        /// <returns>result from database</returns>
        public static T GetReportProperty<T>(this ModelObject modelObject, string name)
        {
            return GetReportProperty<T>(modelObject, name, out _);
        }

        /// <summary>
        /// Gets a User Defined Attribute (UDA) property from a model object with generic type conversion.
        /// </summary>
        /// <typeparam name="T">The type to convert the property to (String, Int, or Double).</typeparam>
        /// <param name="modelObject">Model object to get the UDA property from.</param>
        /// <param name="name">Name of the UDA attribute.</param>
        /// <param name="isSuccess">Indicates whether the property was successfully retrieved.</param>
        /// <returns>The UDA property value converted to the specified type.</returns>
        public static T GetUDAProperty<T>(this ModelObject modelObject, string name, out bool isSuccess)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (typeof(T) == typeof(string))
            {
                string value = string.Empty;
                isSuccess = modelObject.GetUserProperty(name, ref value);
                return (T)converter.ConvertTo(value, typeof(T));
            }
            else if (typeof(T) == typeof(int))
            {
                int value = int.MinValue;
                isSuccess = modelObject.GetUserProperty(name, ref value);
                return (T)converter.ConvertTo(value, typeof(T));
            }
            else if (typeof(T) == typeof(double))
            {
                double value = double.MinValue;
                isSuccess = modelObject.GetUserProperty(name, ref value);
                return (T)converter.ConvertTo(value, typeof(T));
            }
            else
            {
                isSuccess = false;
                return default;
            }
        }

        /// <summary>
        /// Removes all User Defined Attributes (UDA) from a model object by setting them to null or minimum values.
        /// </summary>
        /// <param name="modelObject">Model object to remove UDAs from.</param>
        public static void RemoveAllUDA(this ModelObject modelObject)
        {
            Hashtable  hashtable = new Hashtable();
            modelObject.GetAllUserProperties(ref hashtable);
            foreach (DictionaryEntry item in hashtable)
            {
                string nameUDA = item.Key.ToString();
                if (item.Value is string)
                    modelObject.SetUserProperty(nameUDA, null);
                if (item.Value is int)
                    modelObject.SetUserProperty(nameUDA, int.MinValue);
                if(item.Value is double)
                    modelObject.SetUserProperty (nameUDA, -2147483648.0);
            }
        }
        /// <summary>
        /// Gets a model object by its GUID string.
        /// </summary>
        /// <param name="model">The Tekla Structures model.</param>
        /// <param name="guid">The GUID string of the object to retrieve.</param>
        /// <returns>The model object with the specified GUID.</returns>
        public static ModelObject GetObjectByGuid(this Tekla.Structures.Model.Model model, string guid)
        {
            return model.SelectModelObject(model.GetIdentifierByGUID(guid));
        }
        /// <summary>
        /// Gets a model object by its GUID.
        /// </summary>
        /// <param name="model">The Tekla Structures model.</param>
        /// <param name="guid">The GUID of the object to retrieve.</param>
        /// <returns>The model object with the specified GUID.</returns>
        public static ModelObject GetObjectByGuid(this Tekla.Structures.Model.Model model, Guid guid)
        {
            return model.SelectModelObject(model.GetIdentifierByGUID(guid.ToString()));
        }

        /// <summary>
        /// Determines whether a model object is a connection-type object (Component, Connection, Detail, Seam, CustomPart, or RebarSplice).
        /// </summary>
        /// <param name="obj">The model object to check.</param>
        /// <returns>True if the object is a connection type; otherwise, false.</returns>
        public static bool IsConnectionObject(this ModelObject obj)
        {
            bool result = false;
            switch (obj.GetType().Name)
            {
                case "RebarSplice":
                case "Component":
                case "Connection":
                case "Detail":
                case "Seam":
                case "CustomPart":
                    result = true;
                    break;
            }
            return result;
        }
        /// <summary>
        /// Determines whether a model object is an associative object (bolts, welds, rebars, loads, fittings, cuts, etc.).
        /// </summary>
        /// <param name="obj">The model object to check.</param>
        /// <returns>True if the object is an associative type; otherwise, false.</returns>
        public static bool IsAssociativeObject(this ModelObject obj)
        {
            bool result = false;
            switch (obj.GetType().Name)
            {
                case "BoltArray":
                case "BoltCircle":
                case "BoltXYList":
                case "BooleanPart":
                case "CutPlane":
                case "EdgeChamfer":
                case "Fitting":
                case "CircleRebar":
                case "CircleRebarGroup":
                case "RebarGroup":
                case "RebarMesh":
                case "RebarStrand":
                case "SingleRebar":
                case "CurvedRebarGroup":
                case "LoadArea":
                case "LoadGroup":
                case "LoadLine":
                case "LoadPoint":
                case "LoadUniform":
                case "SurfaceTreatment":
                case "LogicalWeld":
                case "PolygonWeld":
                case "Weld":
                    result = true;
                    break;
            }
            return result;
        }
    }
}
