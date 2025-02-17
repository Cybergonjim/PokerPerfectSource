using PokerPerfect.CoreBusiness;
using PokerPerfect.UseCases.PluginInterfaces;
using System.Text;
using System.Text.Json;

namespace PokerPerfect.Plugins.DataStore.WebApi
{
    // All the code in this file is included in all platforms.
    public class GameWebApiRepository : IGameRepository
    {
        private HttpClient _client;
        private JsonSerializerOptions _serializerOptions;

        public GameWebApiRepository()
        {
            _client = new HttpClient();
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task AddGameAsync(Game game)
        {
            string json = JsonSerializer.Serialize<Game>(game, _serializerOptions);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            Uri uri = new Uri($"{Constants.WebApiBaseUrl}/games");
            await _client.PostAsync(uri, content);
        }

        public async Task DeleteGameAsync(int gameId)
        {
            Uri uri = new Uri($"{Constants.WebApiBaseUrl}/games/{gameId}");
            await _client.DeleteAsync(uri);
        }

        public async Task<Game> GetGameByIdAsync(int gameId)
        {
            Uri uri = new Uri($"{Constants.WebApiBaseUrl}/games/{gameId}");
            CoreBusiness.Game game = null;
            var response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                game = JsonSerializer.Deserialize<Game>(content, _serializerOptions);
            }

            return game;
        }

        public async Task<List<Game>> GetGamesAsync(string filterText)
        {
            var games = new List<CoreBusiness.Game>();

            Uri uri;
            if (string.IsNullOrWhiteSpace(filterText))
                uri = new Uri($"{Constants.WebApiBaseUrl}/games");
            else
                uri = new Uri($"{Constants.WebApiBaseUrl}/games?s={filterText}");

            var response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                games = JsonSerializer.Deserialize<List<Game>>(content, _serializerOptions);
            }

            return games;
            
        }

        public async Task UpdateGameAsync(int gameId, Game game)
        {
            string json = JsonSerializer.Serialize<Game>(game, _serializerOptions);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            Uri uri = new Uri($"{Constants.WebApiBaseUrl}/games/{gameId}");
            await _client.PutAsync(uri, content);
        }

    }
}