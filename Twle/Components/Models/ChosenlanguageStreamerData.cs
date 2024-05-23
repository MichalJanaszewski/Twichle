using static Twle.Components.Utils.Properties;

namespace Twle.Components.Models;
public abstract class ChosenlanguageStreamerData(string mostStreamedCategory, string categoryUrl) : StreamerData
{
    public required string MostStreamedCategory { get; init; } = mostStreamedCategory;

    public required string CategoryUrl { get; init; } = categoryUrl;

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

        for (int i = 0; i < IntData.Length - 1; i++)
        {
            result[i] = IntData[i].CompareTo(streamerData.IntData[i]);
        }
        
        result[IntData.Length ] = MostStreamedCategory == streamerData.MostStreamedCategory ? CorrectBackgroundId : WrongBackgroundId;
        
        return result;
    }
}

