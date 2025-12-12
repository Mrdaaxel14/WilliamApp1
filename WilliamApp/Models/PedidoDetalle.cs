using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WilliamApp.Models
{
    public class PedidoDetalle
    {
        [JsonPropertyName("idPedidoDetalle")]
        public int IdPedidoDetalle { get; set; }
        
        [JsonPropertyName("producto")]
        public PedidoProducto Producto { get; set; }
        
        [JsonPropertyName("cantidad")]
        public int Cantidad { get; set; }
        
        [JsonPropertyName("precioUnitario")]
        public decimal PrecioUnitario { get; set; }
    }

    public class PedidoProducto
    {
        [JsonPropertyName("idProducto")]
        public int IdProducto { get; set; }
        
        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }
        
        [JsonPropertyName("precio")]
        public decimal Precio { get; set; }
        
        [JsonPropertyName("marca")]
        public string Marca { get; set; }
        
        [JsonPropertyName("imagenPrincipal")]
        public string ImagenPrincipal { get; set; }
        
        public string ImagenMostrar => string.IsNullOrEmpty(ImagenPrincipal)
            ? "https://via.placeholder.com/100x100.png?text=Sin+Imagen"
            : ImagenPrincipal;
    }
}

