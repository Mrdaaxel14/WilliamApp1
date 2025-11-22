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
        protected readonly JsonSerializerOptions jsonOptions;
        protected const string BASE_URL = "http://10.0.2.2:5185/api/";
        //protected const string BASE_URL = "http://localhost:5185/api/";
        public ApiService()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(BASE_URL);

            jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            ConfigureAuthorization();
        }

        private void ConfigureAuthorization()
        {
            if (!string.IsNullOrEmpty(Settings.Token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", Settings.Token);
            }
            else
            {
                client.DefaultRequestHeaders.Authorization = null;
            }
        }

        protected async Task<T> GetAsync<T>(string url)
        {
            ConfigureAuthorization();

            using var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, jsonOptions);
        }

        protected async Task<bool> PostAsync(string url, object data)
        {
            ConfigureAuthorization();

            var json = JsonSerializer.Serialize(data);
            var response = await client.PostAsync(url,
                new StringContent(json, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }
    }
}