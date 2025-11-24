using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WilliamApp.Models
{
    public class PerfilUsuario
    {
        [JsonPropertyName("usuario")]
        public Usuario Usuario { get; set; }

        [JsonPropertyName("metodosPago")]
        public List<MetodoPago> MetodosPago { get; set; }

        [JsonPropertyName("direcciones")]
        public List<Direccion> Direcciones { get; set; }
    }
}