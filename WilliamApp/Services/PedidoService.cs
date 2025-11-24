using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WilliamApp.Models;

namespace WilliamApp.Services
{
    public class PedidoService : ApiService
    {
        public async Task<bool> CrearPedido()
        {
            return await PostAsync("pedido/crear", new { });
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
            var resp = await GetAsync<ApiResponse<List<Pedido>>>("pedido/mis-pedidos");
            return resp.response;
        }
    }
}