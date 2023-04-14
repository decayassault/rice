using System.ComponentModel.DataAnnotations;
namespace Own.Database
{
    public partial class LoginLog
    {
        [Key]
        public long Id { get; set; }
        public long AccountIdentifier { get; set; }
        public long IpHash { get; set; }
    }
}
