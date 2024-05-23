namespace Twle.Components.Models;

public abstract class StreamerData
{
    public int Id { get; set; }
    //HOURS WATCHED, AVG VIEWERS, TOTAL FOLLOWERS
    public int[] IntData { get; set; } = [];
    
    public abstract int[] CompareStreamerData(Object streamerData);
}

