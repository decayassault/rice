using System.ComponentModel.DataAnnotations;
namespace Own.Database
{
    public partial class Account
    {
        [Key]
        public long Id { get; set; }
        public string Nick { get; set; }
        public long Identifier { get; set; }
        public long Passphrase { get; set; }
        public long SecretHash { get; set; }
    }
}
