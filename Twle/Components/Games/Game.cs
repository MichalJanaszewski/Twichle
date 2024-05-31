using System.Text;
using System.Text.Json;
using Twle.Components.Models;
using static Twle.Components.Utils.Properties;

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
        private readonly string _gameMode;
        private int _streamersCount = 0;
        private bool locStorageLoaded = false;
        
        private  HttpClient _httpClient = new HttpClient();

        public Func<Task> SetWinningFlag { get; set; }

        public Game(string profilesApiUrl, string gameMode)
        {
            this._profilesApiUrl = profilesApiUrl;
            this._gameMode = gameMode;
        }

        public async Task<int> LoadLocStorageAsync(string? progresValue)
        {
            if (progresValue == null)
            {
                locStorageLoaded = true;
                return _streamersCount;
            }

            string[] splitedStoreage = progresValue.Substring(0, progresValue.LastIndexOf(';')).Split(';');

            if (splitedStoreage.Length > 0 && Int32.Parse(splitedStoreage[splitedStoreage.Length - 1]) == WinnerId)
            {
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
            }

            locStorageLoaded = true;
            return _streamersCount;
        }

        public async Task LoadStreamersProfilesAsync()
        {
            using HttpResponseMessage response = await _httpClient.GetAsync(_profilesApiUrl),
                winnerResponse = await _httpClient.GetAsync(_profilesApiUrl + "/WinnerId");
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
            using HttpResponseMessage response = await _httpClient.GetAsync($"{_profilesApiUrl}/{streamerId}");
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

        public async Task<(string? gameHistory, int streamersCount)> AddStreamer(int id)
        {
            string? valueToSave = null;
            if (StreamerProfiles.Any(profile => profile.Id == id))
            {
                _streamersCount++;
                valueToSave = await LoadStreamerById(id);
                StreamerProfiles.RemoveAt(StreamerProfiles.FindIndex(profile => profile.Id == id));
                if (id == WinnerId)
                {
                    AlreadyWon = true;
                    await SetWinningFlag();
                    await PostStats();
                }
            }

            OnStreamerListChanged?.Invoke();
            return (valueToSave, _streamersCount);
        }

        public async Task<Streamer?> GetWinningStreamer()
        {
            using HttpResponseMessage response = await _httpClient.GetAsync($"{_profilesApiUrl}/{WinnerId}");
            response.EnsureSuccessStatusCode();
            
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

        private async Task PostStats()
        {
            if (!locStorageLoaded)
            {
                return;
            }

            Stats stats = new Stats(_streamersCount, _gameMode);
            try
            {
                var data = JsonSerializer.Serialize(stats);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(MainApiUrl + PostStastsUrl, content);
            }
            catch (Exception ex)
            {
                // ignored
            }
        }
    }
}
