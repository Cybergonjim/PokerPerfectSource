using PokerPerfect.CoreBusiness;
using PokerPerfect.UseCases.PluginInterfaces;
using System.Text;
using System.Text.Json;

namespace PokerPerfect.Plugins.DataStore.WebApi
{
    // All the code in this file is included in all platforms.
    public class ChipsetWebApiRepository : IChipsetRepository
    {
        private HttpClient _client;
        private JsonSerializerOptions _serializerOptions;

        public ChipsetWebApiRepository()
        {
            _client = new HttpClient();
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task AddChipsetAsync(Chipset chipset)
        {
            string json = JsonSerializer.Serialize<Chipset>(chipset, _serializerOptions);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            Uri uri = new Uri($"{Constants.WebApiBaseUrl}/chipsets");
            await _client.PostAsync(uri, content);
        }

        public async Task DeleteChipsetAsync(int chipsetId)
        {
            Uri uri = new Uri($"{Constants.WebApiBaseUrl}/chipsets/{chipsetId}");
            await _client.DeleteAsync(uri);
        }

        public async Task RebuyChipsetAsync(int chipsetId)
        {
            Uri uri = new Uri($"{Constants.WebApiBaseUrl}/chipsets/{chipsetId}");
            await Task.Run(() =>
            {
            });
        }

        public async Task<Chipset> GetChipsetByIdAsync(int chipsetId)
        {
            Uri uri = new Uri($"{Constants.WebApiBaseUrl}/chipsets/{chipsetId}");
            CoreBusiness.Chipset chipset = null;
            var response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                chipset = JsonSerializer.Deserialize<Chipset>(content, _serializerOptions);
            }

            return chipset;
        }

        public async Task<List<Chipset>> GetChipsetsAsync(string filterText)
        {
            var chipsets = new List<CoreBusiness.Chipset>();

            Uri uri;
            if (string.IsNullOrWhiteSpace(filterText))
                uri = new Uri($"{Constants.WebApiBaseUrl}/chipsets");
            else
                uri = new Uri($"{Constants.WebApiBaseUrl}/chipsets?s={filterText}");

            var response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                chipsets = JsonSerializer.Deserialize<List<Chipset>>(content, _serializerOptions);
            }

            return chipsets;
            
        }

        public async Task UpdateChipsetAsync(int chipsetId, Chipset chipset)
        {
            string json = JsonSerializer.Serialize<Chipset>(chipset, _serializerOptions);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            Uri uri = new Uri($"{Constants.WebApiBaseUrl}/chipsets/{chipsetId}");
            await _client.PutAsync(uri, content);
        }

    }
}