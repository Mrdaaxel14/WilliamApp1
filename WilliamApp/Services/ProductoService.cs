using System.Collections.Generic;
using System.Threading.Tasks;
using WilliamApp.Models;
using Microsoft.Maui.Controls;

namespace WilliamApp.Services
{
    public class ProductoService : ApiService
    {
        public async Task<List<Producto>> ObtenerProductos()
        {
            try
            {
                // Endpoint exacto según tu backend
                var productos = await GetAsync<List<Producto>>("producto/lista");
                // DEBUG opcional: mostrar cantidad de productos recibidos
                await Application.Current.MainPage.DisplayAlert("Debug", $"Productos recibidos: {productos?.Count ?? 0}", "OK");
                return productos;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("DebugError", $"Ex: {ex.Message}", "OK");
                return new List<Producto>();
            }
        }
    }
}