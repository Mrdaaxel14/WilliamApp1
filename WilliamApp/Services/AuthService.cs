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
        public async Task<LoginResult> Login(string email, string password)
        {
            var data = new { email, password };
            var json = JsonSerializer.Serialize(data);

            var response = await client.PostAsync("auth/login",
                new StringContent(json, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                return new LoginResult 
                { 
                    Success = false, 
                    Message = "Usuario no encontrado. ¿No tienes cuenta? Regístrate para comenzar."
                };

            var content = await response.Content.ReadAsStringAsync();
            var obj = JsonSerializer.Deserialize<LoginResponse>(content, jsonOptions);

            if (string.IsNullOrEmpty(obj?.Token))
                return new LoginResult 
                { 
                    Success = false, 
                    Message = "Usuario no encontrado. ¿No tienes cuenta? Regístrate para comenzar."
                };

            // Validar que el usuario sea Cliente
            if (obj.User?.Rol != "Cliente")
            {
                // No guardar el token y mostrar mensaje neutral
                return new LoginResult 
                { 
                    Success = false, 
                    Message = "Usuario no encontrado. ¿No tienes cuenta? Regístrate para comenzar."
                };
            }

            // Si es cliente, guardar token y continuar
            Settings.Token = obj.Token;
            return new LoginResult 
            { 
                Success = true, 
                Message = "Login exitoso"
            };
        }

        public async Task<bool> Register(string nombre, string email, string password, string telefono)
        {
            var data = new { nombre, email, password, telefono };
            var json = JsonSerializer.Serialize(data);

            var response = await client.PostAsync("auth/register",
                new StringContent(json, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                // If the API returns 409 Conflict or the error contains email-related message, throw specific exception
                if (response.StatusCode == System.Net.HttpStatusCode.Conflict || 
                    errorContent.Contains("email", StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception("El email ya está registrado");
                }
                return false;
            }

            return true;
        }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
        public Usuario User { get; set; }
    }

    public class LoginResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}