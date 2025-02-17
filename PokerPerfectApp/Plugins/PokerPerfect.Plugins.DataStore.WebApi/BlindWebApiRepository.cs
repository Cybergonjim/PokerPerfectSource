using PokerPerfect.CoreBusiness;
using PokerPerfect.UseCases.PluginInterfaces;
using System.Text;
using System.Text.Json;

namespace PokerPerfect.Plugins.DataStore.WebApi
{
    // All the code in this file is included in all platforms.
    public class BlindWebApiRepository : IBlindRepository
    {
        private HttpClient _client;
        private JsonSerializerOptions _serializerOptions;

        public BlindWebApiRepository()
        {
            _client = new HttpClient();
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task AddBlindAsync(Blind blind)
        {
            string json = JsonSerializer.Serialize<Blind>(blind, _serializerOptions);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            Uri uri = new Uri($"{Constants.WebApiBaseUrl}/blinds");
            await _client.PostAsync(uri, content);
        }

        public async Task DeleteBlindAsync(int blindId)
        {
            Uri uri = new Uri($"{Constants.WebApiBaseUrl}/blinds/{blindId}");
            await _client.DeleteAsync(uri);
        }

        public async Task RebuyBlindAsync(int blindId)
        {
            Uri uri = new Uri($"{Constants.WebApiBaseUrl}/blinds/{blindId}");
            await Task.Run(() =>
            {
            });
        }

        public async Task<Blind> GetBlindByIdAsync(int blindId)
        {
            Uri uri = new Uri($"{Constants.WebApiBaseUrl}/blinds/{blindId}");

            Blind blind = null;

            var response = await _client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                blind = JsonSerializer.Deserialize<Blind>(content, _serializerOptions);
            }

            return blind;
        }

        public async Task<List<Blind>> GetBlindsAsync(string filterText)
        {
            var blinds = new List<CoreBusiness.Blind>();

            Uri uri;
            if (string.IsNullOrWhiteSpace(filterText))
                uri = new Uri($"{Constants.WebApiBaseUrl}/blinds");
            else
                uri = new Uri($"{Constants.WebApiBaseUrl}/blinds?s={filterText}");

            var response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                blinds = JsonSerializer.Deserialize<List<Blind>>(content, _serializerOptions);
            }

            return blinds;
            
        }

        public async Task UpdateBlindAsync(int blindId, Blind blind)
        {
            string json = JsonSerializer.Serialize<Blind>(blind, _serializerOptions);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            Uri uri = new Uri($"{Constants.WebApiBaseUrl}/blinds/{blindId}");
            await _client.PutAsync(uri, content);
        }

    }
}