using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WilliamApp.Models;

namespace WilliamApp.Services
{
    public class CarritoService : ApiService
    {
        public async Task<bool> Agregar(int idProducto, int cantidad)
        {
            return await PostAsync("carrito/agregar", new { idProducto, cantidad });
        }

        public async Task<List<CarritoItem>> ObtenerCarrito()
        {
            var resp = await GetAsync<ApiResponse<List<CarritoItem>>>("carrito/mis-items");
            return resp.response;
        }
        // ✅ NUEVO: Método para eliminar del carrito
        public async Task<bool> Eliminar(int idCarritoDetalle)
        {
            try
            {
                var response = await client.DeleteAsync($"carrito/eliminar/{idCarritoDetalle}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar del carrito: {ex.Message}");
                return false;
            }
        }
    }
}
