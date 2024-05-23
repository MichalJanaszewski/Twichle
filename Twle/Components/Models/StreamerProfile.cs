using static Twle.Components.Utils.Properties;

namespace Twle.Components.Models;

public class StreamerProfile
{
    public StreamerProfile(string imageUrl, string name, int id)
    {
        ImageUrl = imageUrl;
        Name = name;
        Id = id;
    }

    public int Id { get; set; }

    public required string Name { get; set; }
    public required string ImageUrl { get; set; }
    public bool IsVisible { get; set; }

    public int CompareProfiles(StreamerProfile profile)
    {
        return Id == profile.Id ? CorrectBackgroundId : WrongBackgroundId;
    }

    public static int FindStreamer(StreamerProfile[] profiles, string? name) {
        if (name is null)
        {
            return -1;
        }
        foreach (var profile in profiles) {
            if (profile.Name == name) { 
                return profile.Id;
            }
        }
        return -1;
    }
}