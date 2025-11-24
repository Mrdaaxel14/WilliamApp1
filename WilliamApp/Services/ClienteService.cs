using System.Collections.Generic;
using System.Threading.Tasks;
using WilliamApp.Models;

namespace WilliamApp.Services
{
    public class ClienteService : ApiService
    {
        public async Task<PerfilUsuario> ObtenerPerfil()
        {
            var resp = await GetAsync<ApiResponse<PerfilUsuario>>("cliente/perfil");
            return resp.response;
        }

        public async Task<bool> ActualizarPerfil(Usuario usuario)
        {
            return await PostAsync("cliente/actualizar", usuario);
        }

        public async Task<List<MetodoPago>> ObtenerMetodosPago()
        {
            var resp = await GetAsync<ApiResponse<List<MetodoPago>>>("cliente/metodos-pago");
            return resp.response ?? new List<MetodoPago>();
        }

        public async Task<bool> GuardarMetodoPago(MetodoPago metodo)
        {
            var url = metodo.IdMetodoPago > 0
                ? $"cliente/metodos-pago/{metodo.IdMetodoPago}"
                : "cliente/metodos-pago";

            return await PostAsync(url, metodo);
        }

        public async Task<List<Direccion>> ObtenerDirecciones()
        {
            var resp = await GetAsync<ApiResponse<List<Direccion>>>("cliente/direcciones");
            return resp.response ?? new List<Direccion>();
        }

        public async Task<bool> GuardarDireccion(Direccion direccion)
        {
            var url = direccion.IdDireccion > 0
                ? $"cliente/direcciones/{direccion.IdDireccion}"
                : "cliente/direcciones";

            return await PostAsync(url, direccion);
        }
    }
}