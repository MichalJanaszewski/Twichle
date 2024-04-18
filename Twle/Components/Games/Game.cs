using Twle.Api.Models;
using Twle.Components.Models;

namespace Twle.Components.Games
{
    public class Game<T> where T : StreamerData
    {
        public LinkedList<Streamer>? chosenStreamers = new LinkedList<Streamer>();
        public List<StreamerProfile>? streamerList = new List<StreamerProfile>();
        public Boolean alreadyWon = false;
        public string gameHistory = "";
        public int WinnerId { get; set; } = -1;
        protected string profilesApiUrl;
        public Game(string profilesApiUrl)
        {
            this.profilesApiUrl = profilesApiUrl;
        }

        public async Task LoadLocStorageAsync(string? progresValue)
        {

            if (progresValue == null)
            {
                return;
            }

            string[] splitedStoreage = progresValue.Substring(0, progresValue.LastIndexOf(';')).Split(';');

            if (splitedStoreage.Length > 0 && Int32.Parse(splitedStoreage[splitedStoreage.Length - 1]) == WinnerId) {
                alreadyWon = true;
            }

            if (streamerList == null || !streamerList.Any())
            {
                await LoadStreamersProfilesAsync();
            }

            foreach (string stringId in splitedStoreage)
            {
                int id = Int32.Parse(stringId);
                await AddStreamer(id);
            }
        }
        public async Task LoadStreamersProfilesAsync()
        {
                using var httpClient = new HttpClient();
            using HttpResponseMessage response = await httpClient.GetAsync(profilesApiUrl),
                   winnerResponse = await httpClient.GetAsync(profilesApiUrl +"/WinnerId");
            if (response.IsSuccessStatusCode)
            {
                streamerList = await response.Content.ReadFromJsonAsync<List<StreamerProfile>>();
            }

            if (winnerResponse.IsSuccessStatusCode)
            {
                WinnerId = await winnerResponse.Content.ReadFromJsonAsync<int>();
            }

        }

        public async Task<string?> LoadStreamerById(int streamerId)
        {
            using (var httpClient = new HttpClient())
            {
                using (HttpResponseMessage response = await httpClient.GetAsync($"{profilesApiUrl}/{streamerId}"))
                {
                    response.EnsureSuccessStatusCode();
                    StreamerData? data = await response.Content.ReadFromJsonAsync<T>();
                    if (data != null)
                    {
                        chosenStreamers.AddLast(new Streamer(streamerList.Find(profile => profile.Id == streamerId), data));
                        gameHistory += streamerId.ToString() + ";";
                        return gameHistory;
                    }
                }
            }
            return null;
        }

        public event Action OnStreamerListChanged;
        public async Task<string?> AddStreamer(int id)
        {
            string? valueToSave = null;
            if (streamerList != null && streamerList.Any(profile => profile.Id == id))
            {
                valueToSave = await LoadStreamerById(id);
                streamerList.RemoveAt(streamerList.FindIndex(profile => profile.Id == id));
                if (id == WinnerId)
                {
                    alreadyWon = true;
                }
            }
            OnStreamerListChanged?.Invoke();
            return valueToSave;
        }

        public async Task<Streamer> GetWinningStreamer() 
        {
            using (var httpClient = new HttpClient())
            {
                using (HttpResponseMessage response = await httpClient.GetAsync($"{profilesApiUrl}/{WinnerId}"))
                {
                    response.EnsureSuccessStatusCode();
                    StreamerData? data = await response.Content.ReadFromJsonAsync<T>();
                    if (data != null)
                    {
                        Streamer streamer = new Streamer(streamerList.Find(profile => profile.Id == WinnerId), data);
                        return streamer;
                    }
                }
            }

            return null;
        }

    }
}
