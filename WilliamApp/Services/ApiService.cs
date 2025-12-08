using System;
using System.Net.Http;
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
        //Conexion para emulador: 
        protected const string BASE_URL = "http://10.0.2.2:5185/api/";
        //Conexion para Celular fisico:
        //protected const string BASE_URL = "http://192.168.0.20:5185/api/";

        private const int REQUEST_TIMEOUT_SECONDS = 15;
        private const int MAX_RETRIES = 2;

        public ApiService()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            client = new HttpClient(handler)
            {
                BaseAddress = new Uri(BASE_URL),
                Timeout = TimeSpan.FromSeconds(REQUEST_TIMEOUT_SECONDS)
            };

            jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultBufferSize = 4096
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

        protected async Task<T?> GetAsync<T>(string url)
        {
            ConfigureAuthorization();
            Exception? lastException = null;

            for (int i = 0; i < MAX_RETRIES; i++)
            {
                try
                {
                    using var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<T>(content, jsonOptions);
                }
                catch (TaskCanceledException ex)
                {
                    lastException = new Exception("Tiempo de espera agotado.  Verifica tu conexión.", ex);
                    if (i == MAX_RETRIES - 1) throw lastException;
                }
                catch (HttpRequestException ex)
                {
                    lastException = ex;
                    if (i == MAX_RETRIES - 1) throw;
                }
                catch (Exception)
                {
                    throw;
                }
                await Task.Delay(500 * (i + 1));
            }
            throw lastException ?? new Exception("Error desconocido");
        }

        protected async Task<bool> PostAsync(string url, object data)
        {
            ConfigureAuthorization();
            var json = JsonSerializer.Serialize(data, jsonOptions);
            Exception? lastException = null;

            for (int i = 0; i < MAX_RETRIES; i++)
            {
                try
                {
                    var response = await client.PostAsync(url,
                        new StringContent(json, Encoding.UTF8, "application/json"));
                    return response.IsSuccessStatusCode;
                }
                catch (TaskCanceledException ex)
                {
                    lastException = new Exception("Tiempo de espera agotado. Verifica tu conexión.", ex);
                    if (i == MAX_RETRIES - 1) throw lastException;
                }
                catch (HttpRequestException ex)
                {
                    lastException = ex;
                    if (i == MAX_RETRIES - 1) throw;
                }
                catch (Exception)
                {
                    throw;
                }
                await Task.Delay(500 * (i + 1));
            }
            throw lastException ?? new Exception("Error desconocido");
        }

        protected async Task<bool> PutAsync(string url, object data)
        {
            ConfigureAuthorization();
            var json = JsonSerializer.Serialize(data, jsonOptions);
            Exception? lastException = null;

            for (int i = 0; i < MAX_RETRIES; i++)
            {
                try
                {
                    var response = await client.PutAsync(url,
                        new StringContent(json, Encoding.UTF8, "application/json"));
                    return response.IsSuccessStatusCode;
                }
                catch (TaskCanceledException ex)
                {
                    lastException = new Exception("Tiempo de espera agotado.  Verifica tu conexión.", ex);
                    if (i == MAX_RETRIES - 1) throw lastException;
                }
                catch (HttpRequestException ex)
                {
                    lastException = ex;
                    if (i == MAX_RETRIES - 1) throw;
                }
                catch (Exception)
                {
                    throw;
                }
                await Task.Delay(500 * (i + 1));
            }
            throw lastException ?? new Exception("Error desconocido");
        }

        protected async Task<bool> DeleteAsync(string url)
        {
            ConfigureAuthorization();
            Exception? lastException = null;

            for (int i = 0; i < MAX_RETRIES; i++)
            {
                try
                {
                    var response = await client.DeleteAsync(url);
                    return response.IsSuccessStatusCode;
                }
                catch (TaskCanceledException ex)
                {
                    lastException = new Exception("Tiempo de espera agotado.  Verifica tu conexión.", ex);
                    if (i == MAX_RETRIES - 1) throw lastException;
                }
                catch (HttpRequestException ex)
                {
                    lastException = ex;
                    if (i == MAX_RETRIES - 1) throw;
                }
                catch (Exception)
                {
                    throw;
                }
                await Task.Delay(500 * (i + 1));
            }
            throw lastException ?? new Exception("Error desconocido");
        }

        public void Dispose()
        {
            client?.Dispose();
        }
    }
}