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

        [JsonPropertyName("imagenPrincipal")]
        public string ImagenPrincipal { get; set; }

        [JsonPropertyName("galeria")]
        public List<string> Galeria { get; set; } = new List<string>();

        [JsonPropertyName("oCategoria")]
        public Categoria oCategoria { get; set; }

        // Propiedad auxiliar para mostrar imagen por defecto si no hay ninguna
        public string ImagenMostrar => string.IsNullOrEmpty(ImagenPrincipal)
            ? "https://via.placeholder.com/400x400.png?text=Sin+Imagen"
            : ImagenPrincipal;

        // Indica si tiene galería de imágenes
        public bool TieneGaleria => Galeria != null && Galeria.Count > 1;
    }

    public class Categoria
    {
        [JsonPropertyName("idCategoria")]
        public int IdCategoria { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }
    }
}