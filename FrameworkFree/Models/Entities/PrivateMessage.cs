using System.ComponentModel.DataAnnotations;
namespace Own.Database
{
    public partial class PrivateMessage
    {
        [Key]
        public long Id { get; set; }
        public long SenderAccountId { get; set; }
        public long AcceptorAccountId { get; set; }
        public string PrivateText { get; set; }
    }
}
