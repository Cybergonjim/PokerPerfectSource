using PokerPerfect.CoreBusiness;
using PokerPerfect.UseCases.PluginInterfaces;
using System.Text;
using System.Text.Json;

namespace PokerPerfect.Plugins.DataStore.WebApi
{
    // All the code in this file is included in all platforms.
    public class PayoutWebApiRepository : IPayoutRepository
    {
        private HttpClient _client;
        private JsonSerializerOptions _serializerOptions;

        public PayoutWebApiRepository()
        {
            _client = new HttpClient();
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task AddPayoutAsync(Payout payout)
        {
            string json = JsonSerializer.Serialize<Payout>(payout, _serializerOptions);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            Uri uri = new Uri($"{Constants.WebApiBaseUrl}/payouts");
            await _client.PostAsync(uri, content);
        }

        public async Task DeletePayoutAsync(int payoutId)
        {
            Uri uri = new Uri($"{Constants.WebApiBaseUrl}/payouts/{payoutId}");
            await _client.DeleteAsync(uri);
        }

        public async Task RebuyPayoutAsync(int payoutId)
        {
            Uri uri = new Uri($"{Constants.WebApiBaseUrl}/payouts/{payoutId}");
            await Task.Run(() =>
            {
            });
        }

        public async Task<Payout> GetPayoutByIdAsync(int payoutId)
        {
            Uri uri = new Uri($"{Constants.WebApiBaseUrl}/payouts/{payoutId}");
            CoreBusiness.Payout payout = null;
            var response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                payout = JsonSerializer.Deserialize<Payout>(content, _serializerOptions);
            }

            return payout;
        }

        public async Task<List<Payout>> GetPayoutsAsync(string filterText)
        {
            var payouts = new List<CoreBusiness.Payout>();

            Uri uri;
            if (string.IsNullOrWhiteSpace(filterText))
                uri = new Uri($"{Constants.WebApiBaseUrl}/payouts");
            else
                uri = new Uri($"{Constants.WebApiBaseUrl}/payouts?s={filterText}");

            var response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                payouts = JsonSerializer.Deserialize<List<Payout>>(content, _serializerOptions);
            }

            return payouts;
            
        }

        public async Task UpdatePayoutAsync(int payoutId, Payout payout)
        {
            string json = JsonSerializer.Serialize<Payout>(payout, _serializerOptions);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            Uri uri = new Uri($"{Constants.WebApiBaseUrl}/payouts/{payoutId}");
            await _client.PutAsync(uri, content);
        }

    }
}