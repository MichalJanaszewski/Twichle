using static System.Net.WebRequestMethods;

namespace Twle.Components.Utils;

public static class Properties
{
    public static readonly string CorrectDataBackground = "green";
    public static readonly string ToHighDataBackground = "gray";
    public static readonly string ToLowDataBackground = "orange";
    public static readonly string WrongBackground = "red";

    public const int WrongBackgroundId = 2;
    public const int CorrectBackgroundId = 0;
    public const int ToHighDataBackgroundId = 1;
    public const int ToLowDataBackgroundId = -1;

    public static string mainApiUrl = "http://localhost:5196";
    public static string languageModeAccesUrl = "/Language/";
    public static string pictureModeAccesUrl = "/Picture";
    public static string liveModeAccesUrl = "/Live";

}

