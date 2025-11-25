using System.Collections.Generic;
using System.Threading.Tasks;
using WilliamApp.Models;

namespace WilliamApp.Services
{
    public class CategoriaService : ApiService
    {
        public async Task<List<Categoria>> ObtenerCategorias()
        {
            return await GetAsync<List<Categoria>>("categoria/lista");
        }
    }
}