using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WilliamApp.Models;

namespace WilliamApp.Services
{
    public class PedidoService : ApiService
    {
        public async Task<bool> CrearPedido(int? idDireccion = null, int? idMetodoPagoUsuario = null)
        {
            var payload = new
            {
                idDireccion = idDireccion,
                idMetodoPagoUsuario = idMetodoPagoUsuario
            };
            return await PostAsync("pedido/crear", payload);
        }

        public async Task<bool> ConfirmarPedido(int idMetodoPago, int idDireccion, string notas)
        {
            var payload = new
            {
                idMetodoPago,
                idDireccion,
                notas
            };

            return await PostAsync("pedido/confirmar", payload);
        }

        public async Task<List<Pedido>> MisPedidos()
        {
            try
            {
                var resp = await GetAsync<ApiResponse<List<Pedido>>>("pedido/mis-pedidos");
                return resp?.response ?? new List<Pedido>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al obtener pedidos: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado al obtener pedidos: {ex.Message}");
            }

            return new List<Pedido>();
        }
    }
}