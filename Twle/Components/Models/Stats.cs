namespace Twle.Components.Models;

public class Stats
{
    public int StreamersCount { get; set; }
    public string Mode { get; set; }

    public Stats(int streamersCount, string mode)
    {
        StreamersCount = streamersCount;
        Mode = mode;
    }
}