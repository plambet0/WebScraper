using System.Text.Json.Serialization;

namespace WebScraper.Models
{
    public class Product
    {
        [JsonPropertyName("productName")]
        public string Name { get; set; }

        [JsonPropertyName("price")]
        public string Price { get; set; }

        [JsonPropertyName("rating")]
        public string Rating { get; set; }
    }
}
