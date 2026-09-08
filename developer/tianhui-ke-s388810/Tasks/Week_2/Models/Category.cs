using System.ComponentModel.DataAnnotations;

namespace TheaterAdmin.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        // Short code for the category, e.g. "ACT" for Action, "DRA" for Drama
        [Required]
        [StringLength(10)]
        public string Code { get; set; } = string.Empty;

        // Navigation property: one category can have many movies
        public ICollection<Movie>? Movies { get; set; }
    }
}
