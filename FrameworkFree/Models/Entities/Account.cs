using System.ComponentModel.DataAnnotations;
namespace Models
{
    public partial class Account
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(25)]
        public string Nick { get; set; }
        public int Identifier { get; set; }
        public int Passphrase { get; set; }
    }
}
