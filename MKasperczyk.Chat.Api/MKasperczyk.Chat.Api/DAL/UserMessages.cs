namespace MKasperczyk.Chat.Api.DAL
{
    public class UserMessages
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public Message Message { get; set; }
        public int MessageId { get; set; }
    }
}
