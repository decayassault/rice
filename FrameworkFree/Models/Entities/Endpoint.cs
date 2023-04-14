using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Own.Database
{
    [Table("Endpoint_")]
    public partial class Endpoint
    {
        [Key]
        public long Id { get; set; }
        public long ForumId { get; set; }
        public string Name { get; set; }
    }
}
