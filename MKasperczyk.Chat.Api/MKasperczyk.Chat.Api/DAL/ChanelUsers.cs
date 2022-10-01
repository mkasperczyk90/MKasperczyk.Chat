using System.ComponentModel.DataAnnotations;

namespace MKasperczyk.Chat.Api.DAL
{
    public class ChanelRecipients
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public IList<User> Users { get; set; }
        public int ChanelId { get; set; }
        public ChatChanel Chanel { get; set; }
    }
}
