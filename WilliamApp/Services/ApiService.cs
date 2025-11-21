using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WilliamApp.Helpers;

namespace WilliamApp.Services
{
    public class ApiService
    {
        protected readonly HttpClient client;
        //protected const string BASE_URL = "http://10.0.2.2:5185/api/";
        protected const string BASE_URL = "http://localhost:5185";
        public ApiService()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(BASE_URL);

            if (!string.IsNullOrEmpty(Settings.Token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", Settings.Token);
            }
        }

        protected async Task<T> GetAsync<T>(string url)
        {
            var response = await client.GetStringAsync(url);
            return JsonSerializer.Deserialize<T>(response);
        }

        protected async Task<bool> PostAsync(string url, object data)
        {
            var json = JsonSerializer.Serialize(data);
            var response = await client.PostAsync(url,
                new StringContent(json, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }
    }
}
