using System.Text.Json;
using Twle.Components.Models;


namespace Twle.Components.Games
{
    public class Game<T> where T : StreamerData
    {
        public readonly Streamer[] ChosenStreamers = new Streamer[20];
        public int Size;

        public List<StreamerProfile> StreamerProfiles = new List<StreamerProfile>();
        public bool AlreadyWon;
        private string _gameHistory = "";
        private int WinnerId { get; set; } = -1;
        private readonly string _profilesApiUrl;

        public Game(string profilesApiUrl)
        {
            this._profilesApiUrl = profilesApiUrl;
        }

        public async Task<int> LoadLocStorageAsync(string? progresValue)
        {
            int streamersCount = 0;
            if (progresValue == null)
            {
                return streamersCount;
            }

            string[] splitedStoreage = progresValue.Substring(0, progresValue.LastIndexOf(';')).Split(';');

            if (splitedStoreage.Length > 0 && Int32.Parse(splitedStoreage[splitedStoreage.Length - 1]) == WinnerId) {
                AlreadyWon = true;
            }

            if (!StreamerProfiles.Any())
            {
                await LoadStreamersProfilesAsync();
            }
            
            foreach (string stringId in splitedStoreage)
            {
                int id = Int32.Parse(stringId);
                await AddStreamer(id);
                streamersCount++;
            }

            return streamersCount;
        }
        public async Task LoadStreamersProfilesAsync()
        {
                using var httpClient = new HttpClient();
            using HttpResponseMessage response = await httpClient.GetAsync(_profilesApiUrl),
                   winnerResponse = await httpClient.GetAsync(_profilesApiUrl +"/WinnerId");
            if (response.IsSuccessStatusCode)
            {
                List<StreamerProfile>? tmpList = await response.Content.ReadFromJsonAsync<List<StreamerProfile>>();
                if (tmpList is null)
                {
                    throw new HttpRequestException("Failed to retrieve data from the server");
                }
              
                StreamerProfiles = tmpList;
                
            }

            if (winnerResponse.IsSuccessStatusCode)
            {
                WinnerId = await winnerResponse.Content.ReadFromJsonAsync<int>();
            }

        }

        private async Task<string?> LoadStreamerById(int streamerId)
        {

            using var httpClient = new HttpClient();
            using HttpResponseMessage response = await httpClient.GetAsync($"{_profilesApiUrl}/{streamerId}");
            response.EnsureSuccessStatusCode();
            StreamerData? data = await response.Content.ReadFromJsonAsync<T>();
            if (data != null)
            {
                ChosenStreamers[Size] = new Streamer(StreamerProfiles.First(profile => profile.Id == streamerId), data);
                ChosenStreamers[Size++].Profile.IsVisible = true;
                _gameHistory += streamerId.ToString() + ";";
                return _gameHistory;
            }

            return null;
        }

        public event Action? OnStreamerListChanged;
        public async Task<string?> AddStreamer(int id)
        {
            string? valueToSave = null;
            if (StreamerProfiles.Any(profile => profile.Id == id))
            {
                valueToSave = await LoadStreamerById(id);
                StreamerProfiles.RemoveAt(StreamerProfiles.FindIndex(profile => profile.Id == id));
                if (id == WinnerId)
                {
                    AlreadyWon = true;
                }
            }
            OnStreamerListChanged?.Invoke();
            return valueToSave;
        }

        public async Task<Streamer?> GetWinningStreamer() 
        {
            using var httpClient = new HttpClient();
            using HttpResponseMessage response = await httpClient.GetAsync($"{_profilesApiUrl}/{WinnerId}");
            response.EnsureSuccessStatusCode();

            string jsonString = await response.Content.ReadAsStringAsync();
            
            var data = await response.Content.ReadFromJsonAsync<T>();
            if (data != null)
            {
                StreamerProfile? profile = StreamerProfiles.FirstOrDefault(profile => profile.Id == WinnerId);
                if (profile is null) 
                {
                    return null;
                }
                Streamer streamer = new Streamer(profile, data);
                return streamer;
            }

            throw new InvalidOperationException("Can't access server");
        }

    }
}
