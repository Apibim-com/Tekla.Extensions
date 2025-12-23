using System.Collections.Generic;
using System.Globalization;
using TSD = Tekla.Structures.Datatype;


namespace Tekla.Extension;

/// <summary>
/// Extension methods for working with distance strings.
/// </summary>
public static class DistanceExtension
{
    /// <summary>
    /// Parses a distance string and converts it to a list of double values in the current unit type.
    /// </summary>
    /// <param name="distances">The distance string to parse.</param>
    /// <returns>A read-only list of distances as double values.</returns>
    public static IReadOnlyList<double> GetDistances(this string distances)
    {
        List<double> result = new();

        foreach (TSD.Distance distance in TSD.DistanceList.Parse(distances, CultureInfo.InvariantCulture, TSD.Distance.CurrentUnitType))
            result.Add(distance.ConvertTo(TSD.Distance.CurrentUnitType));

        return result;
    }
}
