using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MKasperczyk.Chat.Api.DAL
{
    [Table("Messages")]
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        public int SenderId { get; set; }
        public User Sender { get; set; }
        public int ChanelId { get; set; }
        public ChatChanel Chanel { get; set; }
        public DateTime SendAt { get; set; }
    }
}
