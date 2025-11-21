using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WilliamApp.Models;

namespace WilliamApp.Services
{
    public class ProductoService : ApiService
    {
        public async Task<List<Producto>> ObtenerProductos()
        {
            var apiResp = await GetAsync<ApiResponse<List<Producto>>>("/api/producto/lista");
            return apiResp.response;
        }
    }

    public class ApiResponse<T>
    {
        public string mensaje { get; set; }
        public T response { get; set; }
    }
}

