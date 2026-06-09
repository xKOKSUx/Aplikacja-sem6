using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Character
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;
    }
}
