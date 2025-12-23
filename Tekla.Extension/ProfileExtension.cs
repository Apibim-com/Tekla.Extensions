using System;
using System.Linq;
using Tekla.Structures.Catalogs;

namespace Tekla.Extension;
/// <summary>
/// Helps to work with catalog and get data from profiles.
/// </summary>
public static class ProfileExtension
{
    /// <summary>
    /// Gets a profile property value by attribute name from the Tekla Structures profile catalog.
    /// </summary>
    /// <param name="profile">The profile name to query.</param>
    /// <param name="attribute">The attribute name to retrieve (e.g., "HEIGHT", "WIDTH").</param>
    /// <returns>The profile property value, or 0 if not found.</returns>
    public static double GetProfileProperty(string profile, string attribute)
    {
        LibraryProfileItem libraryProfileItem = new();
        libraryProfileItem.ProfileName = profile;
        if (libraryProfileItem.Select())
        {
            ProfileItemParameter parametr = libraryProfileItem.aProfileItemParameters
                .Cast<ProfileItemParameter>()
                .Where(p => string.Equals(p.Property, attribute, StringComparison.InvariantCulture))
                .FirstOrDefault();

            return parametr is null ? 0 : parametr.Value;
        }
        else
        {
            ParametricProfileItem parametricProfileItem = new();
            parametricProfileItem.ProfilePrefix = profile;
            bool isok = parametricProfileItem.Select();

            if (!isok)
                return 0;

            ProfileItemParameter parametr = parametricProfileItem.aProfileItemParameters
                .Cast<ProfileItemParameter>()
                .Where(p => string.Equals(p.Property, attribute, StringComparison.InvariantCulture))
                .FirstOrDefault();

            return parametr is null ? 0 : parametr.Value;
        }
    }
    /// <summary>
    /// Gets a profile property value by symbol from the Tekla Structures profile catalog.
    /// </summary>
    /// <param name="profile">The profile name to query.</param>
    /// <param name="symbol">The symbol to retrieve (e.g., "h", "b", "tw").</param>
    /// <returns>The profile property value, or 0 if not found.</returns>
    public static double GetProfileSymbol(string profile, string symbol)
    {
        LibraryProfileItem libraryProfileItem = new();
        libraryProfileItem.ProfileName = profile;
        if (libraryProfileItem.Select())
        {
            ProfileItemParameter parametr = libraryProfileItem.aProfileItemParameters
                .Cast<ProfileItemParameter>()
                .Where(p => string.Equals(p.Symbol, symbol, StringComparison.InvariantCulture))
                .FirstOrDefault();

            return parametr is null ? 0 : parametr.Value;
        }
        else
        {
            ParametricProfileItem parametricProfileItem = new();
            parametricProfileItem.ProfilePrefix = profile;
            bool isok = parametricProfileItem.Select();

            if (!isok)
                return 0;

            ProfileItemParameter parametr = parametricProfileItem.aProfileItemParameters
                .Cast<ProfileItemParameter>()
                .Where(p => string.Equals(p.Symbol, symbol, StringComparison.InvariantCulture))
                .FirstOrDefault();

            return parametr is null ? 0 : parametr.Value;
        }
    }
    /// <summary>
    /// Gets the height of a profile from the Tekla Structures profile catalog.
    /// </summary>
    /// <param name="profile">The profile name to query.</param>
    /// <returns>The profile height value.</returns>
    public static double GetProfileHeight(string profile)
    {
        return GetProfileProperty(profile, "HEIGHT");
    }
    /// <summary>
    /// Gets the width of a profile from the Tekla Structures profile catalog.
    /// </summary>
    /// <param name="profile">The profile name to query.</param>
    /// <returns>The profile width value.</returns>
    public static double GetProfileWidth(string profile)
    {
        return GetProfileProperty(profile, "WIDTH");
    }
    /// <summary>
    /// Checks whether a profile exists in the Tekla Structures profile catalog.
    /// </summary>
    /// <param name="profile">The profile name to check.</param>
    /// <returns>True if the profile exists (as either library or parametric profile); otherwise, false.</returns>
    public static bool IsProfileExist(string profile)
    {
        LibraryProfileItem libraryProfileItem = new();
        libraryProfileItem.ProfileName = profile;
        bool isStatic = libraryProfileItem.Select();
        if (isStatic)
            return isStatic;

        ParametricProfileItem parametricProfileItem = new();
        parametricProfileItem.ProfilePrefix = profile;
        return parametricProfileItem.Select();
    }
    /// <summary>
    /// Get width of plate without inserting in tekla. PL10 will return 10.
    /// </summary>
    /// <param name="profile"></param>
    /// <returns>Width of profile</returns>
    public static double GetPlateWidthByProfile(string profile)
    {
        ParametricProfileItem parametric = new();
        parametric.ProfilePrefix = profile;
        parametric.Select();

        foreach (ProfileItemParameter item in parametric.aProfileItemParameters)
        {
            return item.Value;
        }
        return 0;
    }
}
