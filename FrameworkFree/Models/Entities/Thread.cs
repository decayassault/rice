using System.ComponentModel.DataAnnotations;
namespace Own.Database
{
    public partial class Thread
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public long EndpointId { get; set; }
    }
}
