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
            return await PostAsync("/api/pedido/crear", new { });
        }

        public async Task<List<Pedido>> MisPedidos()
        {
            var resp = await GetAsync<ApiResponse<List<Pedido>>>("/api/pedido/mis-pedidos");
            return resp.response;
        }
    }
}
