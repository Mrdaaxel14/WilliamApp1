using System.Text.Json.Serialization;

namespace WilliamApp.Models
{
    public class Direccion
    {
        [JsonPropertyName("idDireccion")]
        public int IdDireccion { get; set; }

        [JsonPropertyName("calle")]
        public string Calle { get; set; }

        [JsonPropertyName("numero")]
        public string Numero { get; set; }

        [JsonPropertyName("ciudad")]
        public string Ciudad { get; set; }

        [JsonPropertyName("provincia")]
        public string Provincia { get; set; }

        [JsonPropertyName("codigoPostal")]
        public string CodigoPostal { get; set; }

        [JsonPropertyName("referencia")]
        public string Referencia { get; set; }

        public string Etiqueta => $"{Calle} {Numero}, {Ciudad} ({Provincia})";
    }
}