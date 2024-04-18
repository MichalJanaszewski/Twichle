using static Twle.Components.Utils.Properties;

namespace Twle.Components.Models;

public class MainModeStreamerData : StreamerData
{
    public string Language { get; set; }
    public override int[] CompareStreamerData(Object streamerData)
    {
        if (streamerData.GetType() != this.GetType()) {
            throw new ArgumentException("streamerData should be an instance of MainModeStreamerData");
        }

         return CompareStreamerData((MainModeStreamerData) streamerData);
    }

    private int[] CompareStreamerData(MainModeStreamerData streamerData) {

        int[] result = new int[IntData.Length + 1];
        result[0] = Language == streamerData.Language ? CorrectBackgroundId : WrongBackgroundId;

        for (int i = 0; i < IntData.Length; i++)
        {
            result[i + 1] = IntData[i].CompareTo(streamerData.IntData[i]);
        }

        return result;
    }

}
