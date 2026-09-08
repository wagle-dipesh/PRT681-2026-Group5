using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TheaterAdminWebApi.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string Code { get; set; } = string.Empty;

        // Ignored in JSON responses to avoid a Category -> Movies -> Category circular
        // reference when serializing (the API returns movies with a flat CategoryName instead).
        [JsonIgnore]
        public ICollection<Movie>? Movies { get; set; }
    }
}
