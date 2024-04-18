using static Twle.Components.Utils.Properties;

namespace Twle.Api.Models;

public class StreamerProfile
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string ImageUrl { get; set; }

    public int CompareProfiles(StreamerProfile profile)
    {
        return Id == profile.Id ? CorrectBackgroundId : WrongBackgroundId;
    }
}