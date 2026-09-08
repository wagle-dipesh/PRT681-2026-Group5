using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TheaterAdminWebApi.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [StringLength(150)]
        public string Director { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string ContactEmail { get; set; } = string.Empty;

        [Required]
        public Language Language { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        [JsonIgnore] // the API exposes CategoryName directly instead of the full nested object
        public Category? Category { get; set; }

        // Not mapped to a database column — populated manually in the controller so the
        // jQuery client can display the category name without a second API call per movie.
        [NotMapped]
        public string? CategoryName { get; set; }
    }
}
