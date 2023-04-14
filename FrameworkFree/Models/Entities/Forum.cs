using System.ComponentModel.DataAnnotations;
namespace Own.Database
{
    public partial class Forum
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
