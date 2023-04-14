using System.ComponentModel.DataAnnotations;
namespace Own.Database
{
    public partial class BlockedIpHash
    {
        [Key]
        public long Id { get; set; }
        public long IpHash { get; set; }
    }
}