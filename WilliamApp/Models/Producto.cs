using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace WilliamApp.Models
{
    public class Producto
    {
        [JsonPropertyName("idProducto")]
        public int IdProducto { get; set; }

        [JsonPropertyName("codigoBarra")]
        public string CodigoBarra { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }

        [JsonPropertyName("marca")]
        public string Marca { get; set; }

        [JsonPropertyName("idCategoria")]
        public int IdCategoria { get; set; }

        [JsonPropertyName("precio")]
        public decimal Precio { get; set; }

        [JsonPropertyName("oCategoria")]
        public Categoria oCategoria { get; set; }
    }

    public class Categoria
    {
        [JsonPropertyName("idCategoria")]
        public int IdCategoria { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }
    }
}