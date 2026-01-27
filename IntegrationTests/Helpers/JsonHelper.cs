using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace dotnet_Warehouse_Management_System.Tests.IntegrationTests
{
    public static class JsonHelper
    {
        // Opzioni comuni per tutti i test
        public static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };

        // Extension method per leggere JSON con enum come stringhe
        public static Task<T?> ReadFromJsonWithEnumAsync<T>(this HttpContent content)
        {
            return content.ReadFromJsonAsync<T>(Options);
        }

        // Extension method per inviare JSON con enum come stringhe
        public static Task<HttpResponseMessage> PostAsJsonWithEnumAsync<T>(this HttpClient client, string url, T obj)
        {
            var content = JsonContent.Create(obj, options: Options);
            return client.PostAsync(url, content);
        }

        public static Task<HttpResponseMessage> PutAsJsonWithEnumAsync<T>(this HttpClient client, string url, T obj)
        {
            var content = JsonContent.Create(obj, options: Options);
            return client.PutAsync(url, content);
        }
    }
}
