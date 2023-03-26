using System.ComponentModel.DataAnnotations;
namespace Models
{
    public partial class Thread
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(99)]
        public string Name { get; set; }
        public int EndpointId { get; set; }
    }
}
