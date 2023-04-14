using System.ComponentModel.DataAnnotations;
namespace Own.Database
{
    public partial class Msg
    {
        [Key]
        public long Id { get; set; }
        public long ThreadId { get; set; }
        public long AccountId { get; set; }
        public string MsgText { get; set; }
    }
}
