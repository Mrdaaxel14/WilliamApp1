using System.Text.Json.Serialization;

namespace WilliamApp.Models
{
    public class ApiResponse<T>
    {
        [JsonPropertyName("mensaje")]
        public string mensaje { get; set; }

        [JsonPropertyName("response")]
        public T response { get; set; }
    }
}