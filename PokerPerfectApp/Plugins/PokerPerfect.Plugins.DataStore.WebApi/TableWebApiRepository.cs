using PokerPerfect.CoreBusiness;
using PokerPerfect.UseCases.PluginInterfaces;
using System.Text;
using System.Text.Json;

namespace PokerPerfect.Plugins.DataStore.WebApi
{
    // All the code in this file is included in all platforms.
    public class TableWebApiRepository : ITableRepository
    {
        private HttpClient _client;
        private JsonSerializerOptions _serializerOptions;

        public TableWebApiRepository()
        {
            _client = new HttpClient();
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task AddTableAsync(Table table)
        {
            string json = JsonSerializer.Serialize<Table>(table, _serializerOptions);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            Uri uri = new Uri($"{Constants.WebApiBaseUrl}/tables");
            await _client.PostAsync(uri, content);
        }

        public async Task DeleteTableAsync(int tableId)
        {
            Uri uri = new Uri($"{Constants.WebApiBaseUrl}/tables/{tableId}");
            await _client.DeleteAsync(uri);
        }

        public async Task RebuyTableAsync(int tableId)
        {
            Uri uri = new Uri($"{Constants.WebApiBaseUrl}/tables/{tableId}");
            await Task.Run(() =>
            {
            });
        }

        public async Task<Table> GetTableByIdAsync(int tableId)
        {
            Uri uri = new Uri($"{Constants.WebApiBaseUrl}/tables/{tableId}");
            CoreBusiness.Table table = null;
            var response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                table = JsonSerializer.Deserialize<Table>(content, _serializerOptions);
            }

            return table;
        }

        public async Task<List<Table>> GetTablesAsync(string filterText)
        {
            var tables = new List<CoreBusiness.Table>();

            Uri uri;
            if (string.IsNullOrWhiteSpace(filterText))
                uri = new Uri($"{Constants.WebApiBaseUrl}/tables");
            else
                uri = new Uri($"{Constants.WebApiBaseUrl}/tables?s={filterText}");

            var response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                tables = JsonSerializer.Deserialize<List<Table>>(content, _serializerOptions);
            }

            return tables;
            
        }

        public async Task UpdateTableAsync(int tableId, Table table)
        {
            string json = JsonSerializer.Serialize<Table>(table, _serializerOptions);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            Uri uri = new Uri($"{Constants.WebApiBaseUrl}/tables/{tableId}");
            await _client.PutAsync(uri, content);
        }

    }
}