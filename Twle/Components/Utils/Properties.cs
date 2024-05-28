using static System.Net.WebRequestMethods;

namespace Twle.Components.Utils;

public static class Properties
{
    public static readonly string CorrectDataBackground = "green";
    public static readonly string ToHighDataBackground = "WrongAnswerToHigh.jpg";
    public static readonly string ToLowDataBackground = "WrongAnswerToLow.jpg";
    public static readonly string WrongBackground = "#D22D45";

    public const int WrongBackgroundId = 2;
    public const int CorrectBackgroundId = 0;
    public const int ToHighDataBackgroundId = 1;
    public const int ToLowDataBackgroundId = -1;

    public const string MainApiUrl = "https://localhost:7017";
    public static string MainModeApiUrl = "/MainMode";
    public const string LanguageModeAccesUrl = "/LanguageMode/";
    public const string PictureModeAccesUrl = "/PictureMode";
    public const string LiveModeAccesUrl = "/LiveMode";


    public const double TimeFlipDuration = 0.5; //0.5s
}

