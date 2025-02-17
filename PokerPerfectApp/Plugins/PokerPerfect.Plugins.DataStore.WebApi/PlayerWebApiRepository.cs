using PokerPerfect.CoreBusiness;
using PokerPerfect.UseCases.PluginInterfaces;
using System.Text;
using System.Text.Json;

namespace PokerPerfect.Plugins.DataStore.WebApi
{
  // All the code in this file is included in all platforms.
  public class PlayerWebApiRepository : IPlayerRepository
  {
    private HttpClient _client;
    private JsonSerializerOptions _serializerOptions;

    public PlayerWebApiRepository()
    {
      _client = new HttpClient();
      _serializerOptions = new JsonSerializerOptions
      {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
      };
    }

    public async Task AddPlayerAsync(Player player)
    {
      string json = JsonSerializer.Serialize<Player>(player, _serializerOptions);
      StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

      Uri uri = new Uri($"{Constants.WebApiBaseUrl}/players");
      await _client.PostAsync(uri, content);
    }

    public async Task DeletePlayerAsync(int playerId)
    {
      Uri uri = new Uri($"{Constants.WebApiBaseUrl}/players/{playerId}");
      await _client.DeleteAsync(uri);
    }

    public async Task RebuyPlayerAsync(int playerId)
    {
      Uri uri = new Uri($"{Constants.WebApiBaseUrl}/players/{playerId}");
      await Task.Run(() =>
      {
      });
    }

    public async Task<Player> GetPlayerByIdAsync(int playerId)
    {
      Uri uri = new Uri($"{Constants.WebApiBaseUrl}/players/{playerId}");
      CoreBusiness.Player player = null;
      var response = await _client.GetAsync(uri);
      if (response.IsSuccessStatusCode)
      {
        string content = await response.Content.ReadAsStringAsync();
        player = JsonSerializer.Deserialize<Player>(content, _serializerOptions);
      }

      return player;
    }

    public async Task<List<Player>> GetPlayersAsync(string filterText)
    {
      var players = new List<CoreBusiness.Player>();

      Uri uri;
      if (string.IsNullOrWhiteSpace(filterText))
        uri = new Uri($"{Constants.WebApiBaseUrl}/players");
      else
        uri = new Uri($"{Constants.WebApiBaseUrl}/players?s={filterText}");

      var response = await _client.GetAsync(uri);
      if (response.IsSuccessStatusCode)
      {
        string content = await response.Content.ReadAsStringAsync();
        players = JsonSerializer.Deserialize<List<Player>>(content, _serializerOptions);
      }

      return players;

    }

    public async Task UpdatePlayerAsync(int playerId, Player player)
    {
      string json = JsonSerializer.Serialize<Player>(player, _serializerOptions);
      StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

      Uri uri = new Uri($"{Constants.WebApiBaseUrl}/players/{playerId}");
      await _client.PutAsync(uri, content);
    }

  }
}