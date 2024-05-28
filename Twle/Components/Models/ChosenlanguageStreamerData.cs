using System.Text.Json.Serialization;
using static Twle.Components.Utils.Properties;

namespace Twle.Components.Models;
public class ChosenlanguageStreamerData : StreamerData
{
    public string MostStreamedCategory { get; set; }
    public string CategoryUrl { get; set; }

    public override int[] CompareStreamerData(Object streamerData)
    {
        if (streamerData.GetType() != this.GetType())
        {
            throw new ArgumentException("streamerData should be an instance of MainModeStreamerData");
        }

        return CompareStreamerData((ChosenlanguageStreamerData)streamerData);
    }

    private int[] CompareStreamerData(ChosenlanguageStreamerData streamerData)
    {
        int[] result = new int[IntData.Length + 1];

        for (int i = 0; i < IntData.Length; i++)
        {
            result[i] = IntData[i].CompareTo(streamerData.IntData[i]);
        }
        
        result[^1] = MostStreamedCategory == streamerData.MostStreamedCategory ? CorrectBackgroundId : WrongBackgroundId;
        
        return result;
    }
}

