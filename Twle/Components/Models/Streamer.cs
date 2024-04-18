using Twle.Api.Models;
using static Twle.Components.Utils.Properties;
namespace Twle.Components.Models;

public class Streamer
{
    public StreamerProfile Profile { get; set; }
    public StreamerData Data { get; set; }

    public Streamer(StreamerProfile profile, StreamerData data) { 
        this.Profile = profile;
        this.Data = data;   
    }


    private int[] CompareStreames(Streamer streamer)
    {
        int[] comperedData = Data.CompareStreamerData(streamer.Data);
        int comperedProfile = Profile.CompareProfiles(streamer.Profile);

        int[] result = new int[comperedData.Length + 1];
        result[0] = comperedProfile;
        Array.Copy(comperedData, 0, result, 1, comperedData.Length);

        return result;
    }

    public string[] GetBackrounds(Streamer streamer)
    {
        int[] comparedData = CompareStreames(streamer);
        string[] result = new string[comparedData.Length];

        for (int i = 0; i < comparedData.Length; i++)
        {
            result[i] = GetBackround(comparedData[i]);
        }

        return result;
    }

    private string GetBackround(int data)
    {
        switch (data)
        {
            case CorrectBackgroundId:
                return CorrectDataBackground;
            case ToHighDataBackgroundId:
                return ToHighDataBackground;
            case ToLowDataBackgroundId:
                return ToLowDataBackground;
            case WrongBackgroundId:
                return WrongBackground;
        }

        return "";
    }
}

