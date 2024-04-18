using Twle.Components.Pages;

namespace Twle.Components.Models;
public class ChosenlanguageStreamerData : StreamerData
{
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
        int[] result = new int[IntData.Length];

        for (int i = 0; i < IntData.Length; i++)
        {
            result[i] = IntData[i].CompareTo(streamerData.IntData[i]);
        }

        return result;
    }

}

