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
            return await PostAsync("/api/carrito/agregar", new { idProducto, cantidad });
        }

        public async Task<List<CarritoItem>> ObtenerCarrito()
        {
            var resp = await GetAsync<ApiResponse<List<CarritoItem>>>("/api/carrito/mis-items");
            return resp.response;
        }
    }
}
