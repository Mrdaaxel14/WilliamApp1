using System.Text.Json.Serialization;

namespace WilliamApp.Models
{
    public class MetodoPago
    {
        [JsonPropertyName("idMetodoPagoUsuario")]
        public int IdMetodoPago { get; set; }

        [JsonPropertyName("metodo")]
        public string Metodo { get; set; }

        [JsonPropertyName("titular")]
        public string Titular { get; set; }

        [JsonPropertyName("ultimos4")]
        public string Ultimos4 { get; set; }

        [JsonPropertyName("expiracion")]
        public string Expiracion { get; set; }

        public string Resumen => string.IsNullOrWhiteSpace(Ultimos4)
            ? Metodo ?? "Sin definir"
            : $"{Metodo} ****{Ultimos4} - {Titular}";
    }
}