using System.ComponentModel.DataAnnotations;

namespace MKasperczyk.Chat.Api.DAL
{
    public class ChatChanel
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public IList<Message> Messages { get; set; }
        public IList<ChanelRecipients> Recipients { get; set; }
    }
}
