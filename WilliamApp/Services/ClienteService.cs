using System.Collections.Generic;
using System.Threading.Tasks;
using WilliamApp.Models;

namespace WilliamApp.Services
{
    public class ClienteService : ApiService
    {
        // Obtener perfil del usuario autenticado
        public async Task<Usuario> ObtenerPerfil()
        {
            var resp = await GetAsync<ApiResponse<Usuario>>("perfil");
            return resp?.response;
        }

        // Actualizar perfil
        public async Task<bool> ActualizarPerfil(Usuario usuario)
        {
            return await PutAsync("perfil", new
            {
                nombre = usuario.Nombre,
                email = usuario.Email,
                telefono = usuario.Telefono
            });
        }

        // Obtener métodos de pago del usuario
        public async Task<List<MetodoPago>> ObtenerMetodosPago()
        {
            var resp = await GetAsync<ApiResponse<List<MetodoPago>>>("metodospagousuario/mios");
            return resp?.response ?? new List<MetodoPago>();
        }

        // Guardar método de pago (crear o actualizar)
        public async Task<bool> GuardarMetodoPago(MetodoPago metodo)
        {
            var payload = new
            {
                metodo = metodo.Metodo,
                titular = metodo.Titular,
                ultimos4 = metodo.Ultimos4,
                expiracion = metodo.Expiracion
            };

            if (metodo.IdMetodoPago > 0)
            {
                return await PutAsync($"metodospagousuario/{metodo.IdMetodoPago}", payload);
            }
            else
            {
                return await PostAsync("metodospagousuario", payload);
            }
        }

        // Eliminar método de pago
        public async Task<bool> EliminarMetodoPago(int id)
        {
            return await DeleteAsync($"metodospagousuario/{id}");
        }

        // Obtener direcciones del usuario
        public async Task<List<Direccion>> ObtenerDirecciones()
        {
            var resp = await GetAsync<ApiResponse<List<Direccion>>>("direcciones/mias");
            return resp?.response ?? new List<Direccion>();
        }

        // Guardar dirección (crear o actualizar)
        public async Task<bool> GuardarDireccion(Direccion direccion)
        {
            var payload = new
            {
                provincia = direccion.Provincia,
                ciudad = direccion.Ciudad,
                calle = direccion.Calle,
                numero = direccion.Numero,
                codigoPostal = direccion.CodigoPostal
            };

            if (direccion.IdDireccion > 0)
            {
                return await PutAsync($"direcciones/{direccion.IdDireccion}", payload);
            }
            else
            {
                return await PostAsync("direcciones", payload);
            }
        }

        // Eliminar dirección
        public async Task<bool> EliminarDireccion(int id)
        {
            return await DeleteAsync($"direcciones/{id}");
        }
    }
}