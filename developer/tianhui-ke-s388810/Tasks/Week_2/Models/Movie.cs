using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheaterAdmin.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [StringLength(150)]
        public string Director { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Contact Email Address")]
        public string ContactEmail { get; set; } = string.Empty;

        [Required]
        public Language Language { get; set; }

        // Foreign key to Category — a movie must have exactly one category (NOT NULL, as required)
        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }
    }
}
