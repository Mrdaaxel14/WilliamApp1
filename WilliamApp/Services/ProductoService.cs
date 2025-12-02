using System;
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
                var resp = await GetAsync<ApiResponse<List<Producto>>>("producto/lista");
                return resp?.response ?? new List<Producto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener productos: {ex.Message}");
                return new List<Producto>();
            }
        }

        public async Task<Producto> ObtenerProductoDetalle(int id)
        {
            try
            {
                var resp = await GetAsync<ApiResponse<Producto>>($"producto/{id}");
                return resp?.response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener detalle del producto: {ex.Message}");
                return null;
            }
        }
    }
}