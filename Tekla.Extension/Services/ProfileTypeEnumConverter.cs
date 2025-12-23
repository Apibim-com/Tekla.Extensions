using Tekla.Extension.Enums;

namespace Tekla.Extension.Services;

/// <summary>
/// Provides utility methods for converting profile type strings to ProfileType enum values.
/// </summary>
public static class ProfileTypeEnumConverter
{

    /// <summary>
    /// Converts string to ProfileType.
    /// </summary>
    /// <param name="str">String value API is working with.</param>
    public static ProfileType GetProfileTypeFromString(string str)
    {
        return str switch
        {
            "B" => ProfileType.Plate,
            "I" => ProfileType.Ibeam,
            "L" => ProfileType.Angle,
            "U" => ProfileType.Channel,
            "RU" => ProfileType.RoundBar,
            "RO" => ProfileType.RoundTube,
            "M" => ProfileType.RectangularTube,
            "C" => ProfileType.CFChannel,
            "T" => ProfileType.Tbeam,
            "Z" => ProfileType.Zbeam,
            _ => ProfileType.Unknown,
        };
    }

    /// <summary>
    /// Converts ProfileType back to string.
    /// </summary>
    /// <param name="profileType"></param>
    /// <returns>String corresponding to Enum</returns>
    public static string GetStringValueFromProfileType(ProfileType profileType)
    {
        return profileType switch
        {
            ProfileType.Plate => "B",
            ProfileType.Ibeam => "I",
            ProfileType.Angle => "L",
            ProfileType.Channel => "U",
            ProfileType.RoundBar => "RU",
            ProfileType.RoundTube => "RO",
            ProfileType.RectangularTube => "M",
            ProfileType.CFChannel => "C",
            ProfileType.Tbeam => "T",
            ProfileType.Zbeam => "Z",
            _ => "Unknown",
        };
    }
}
