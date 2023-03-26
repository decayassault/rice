using System.ComponentModel.DataAnnotations;
namespace Models
{
    public partial class Forum
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(24)]
        public string Name { get; set; }
    }
}
