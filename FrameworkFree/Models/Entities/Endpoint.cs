using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("Endpoint_")]
    public partial class Endpoint
    {
        [Key]
        public int Id { get; set; }
        public int ForumId { get; set; }
        [Required]
        [StringLength(39)]
        public string Name { get; set; }
    }
}
