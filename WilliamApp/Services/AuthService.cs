using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text;
using System.Text.Json;
using WilliamApp.Helpers;
using WilliamApp.Models;

namespace WilliamApp.Services
{
    public class AuthService : ApiService
    {
        public async Task<bool> Login(string email, string password)
        {
            var data = new { email, password };
            var json = JsonSerializer.Serialize(data);

            var response = await client.PostAsync("/api/auth/login",
                new StringContent(json, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                return false;

            var content = await response.Content.ReadAsStringAsync();
            var obj = JsonSerializer.Deserialize<LoginResponse>(content);

            Settings.Token = obj.Token;
            return true;
        }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
    }
}
