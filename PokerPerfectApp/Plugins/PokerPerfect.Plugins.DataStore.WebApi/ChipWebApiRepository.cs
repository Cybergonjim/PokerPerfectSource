using PokerPerfect.CoreBusiness;
using PokerPerfect.UseCases.PluginInterfaces;
using System.Text;
using System.Text.Json;

namespace PokerPerfect.Plugins.DataStore.WebApi
{
    // All the code in this file is included in all platforms.
    public class ChipWebApiRepository : IChipRepository
    {
        private HttpClient _client;
        private JsonSerializerOptions _serializerOptions;

        public ChipWebApiRepository()
        {
            _client = new HttpClient();
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task AddChipAsync(Chip chip)
        {
            string json = JsonSerializer.Serialize<Chip>(chip, _serializerOptions);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            Uri uri = new Uri($"{Constants.WebApiBaseUrl}/chips");
            await _client.PostAsync(uri, content);
        }

        public async Task DeleteChipAsync(int chipId)
        {
            Uri uri = new Uri($"{Constants.WebApiBaseUrl}/chips/{chipId}");
            await _client.DeleteAsync(uri);
        }

        public async Task RebuyChipAsync(int chipId)
        {
            Uri uri = new Uri($"{Constants.WebApiBaseUrl}/chips/{chipId}");

            await Task.Run(() =>
            {
            });
        }

        public async Task<Chip> GetChipByIdAsync(int chipId)
        {
            Uri uri = new Uri($"{Constants.WebApiBaseUrl}/chips/{chipId}");
            CoreBusiness.Chip chip = null;
            var response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                chip = JsonSerializer.Deserialize<Chip>(content, _serializerOptions);
            }

            return chip;
        }

        public async Task<List<Chip>> GetChipsAsync(string filterText)
        {
            var chips = new List<CoreBusiness.Chip>();

            Uri uri;
            if (string.IsNullOrWhiteSpace(filterText))
                uri = new Uri($"{Constants.WebApiBaseUrl}/chips");
            else
                uri = new Uri($"{Constants.WebApiBaseUrl}/chips?s={filterText}");

            var response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                chips = JsonSerializer.Deserialize<List<Chip>>(content, _serializerOptions);
            }

            return chips;
            
        }

        public async Task UpdateChipAsync(int chipId, Chip chip)
        {
            string json = JsonSerializer.Serialize<Chip>(chip, _serializerOptions);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            Uri uri = new Uri($"{Constants.WebApiBaseUrl}/chips/{chipId}");
            await _client.PutAsync(uri, content);
        }

    }
}