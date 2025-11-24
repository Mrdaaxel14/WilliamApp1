using System;
using System.Text.Json.Serialization;

namespace WilliamApp.Models
{
    public class MetodoPago
    {
        [JsonPropertyName("idMetodoPago")]
        public int IdMetodoPago { get; set; }

        [JsonPropertyName("alias")]
        public string Alias { get; set; }

        [JsonPropertyName("titular")]
        public string Titular { get; set; }

        [JsonPropertyName("numeroEnmascarado")]
        public string NumeroEnmascarado { get; set; }

        [JsonPropertyName("vencimiento")]
        public string Vencimiento { get; set; }

        [JsonPropertyName("marca")]
        public string Marca { get; set; }

        public string Resumen => string.IsNullOrWhiteSpace(NumeroEnmascarado)
            ? Alias
            : $"{Marca} {NumeroEnmascarado} - {Titular}";
    }
}