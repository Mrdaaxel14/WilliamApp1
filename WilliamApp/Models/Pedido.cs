using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WilliamApp.Models
{
    public class Pedido
    {
        [JsonPropertyName("idPedido")]
        public int IdPedido { get; set; }
        
        [JsonPropertyName("fecha")]
        public DateTime Fecha { get; set; }
        
        [JsonPropertyName("total")]
        public decimal Total { get; set; }
        
        [JsonPropertyName("detalles")]
        public List<PedidoDetalle> Detalles { get; set; }
        
        [JsonPropertyName("estadoPedido")]
        public string EstadoPedido { get; set; }
        
        [JsonPropertyName("estadoPago")]
        public string EstadoPago { get; set; }
        
        [JsonPropertyName("cantidadItems")]
        public int CantidadItems { get; set; }
        
        // Propiedades auxiliares para la UI
        public string FechaFormateada => Fecha.ToString("d 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));
        
        public string CantidadItemsTexto => CantidadItems == 1 ? "1 producto" : $"{CantidadItems} productos";
        
        public string ImagenPrincipal
        {
            get
            {
                if (Detalles != null && Detalles.Count > 0 && Detalles[0].Producto?.ImagenPrincipal != null)
                {
                    return Detalles[0].Producto.ImagenPrincipal;
                }
                return "https://via.placeholder.com/100x100.png?text=Sin+Imagen";
            }
        }
        
        public string ColorEstado
        {
            get
            {
                return EstadoPedido?.ToLower() switch
                {
                    "entregado" => "#4CAF50",
                    "enviado" => "#2196F3",
                    "pendiente" => "#FF9800",
                    "cancelado" => "#F44336",
                    _ => "#9E9E9E"
                };
            }
        }
        
        public string DescripcionEstado
        {
            get
            {
                return EstadoPedido?.ToLower() switch
                {
                    "entregado" => "Pedido entregado",
                    "enviado" => "En camino",
                    "pendiente" => "Procesando pedido",
                    "cancelado" => "Pedido cancelado",
                    _ => ""
                };
            }
        }
    }
}

