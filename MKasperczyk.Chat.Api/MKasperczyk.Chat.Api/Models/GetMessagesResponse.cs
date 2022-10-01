namespace MKasperczyk.Chat.Api.Models
{
    public class GetMessagesResponse
    {
        public int MessageId { get; set; }
        public string? Message { get; set; }
        public DateTime SendAt { get; set; }
        public string Type { get; set; }
    }
}
