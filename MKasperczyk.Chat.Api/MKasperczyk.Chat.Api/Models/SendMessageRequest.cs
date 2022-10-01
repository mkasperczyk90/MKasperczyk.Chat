namespace MKasperczyk.Chat.Api.Models
{
    public class SendMessageRequest
    {
        public int Sender { get; set; }
        public int[] Recipients { get; set; }
        public string Message { get; set; }
    }
}
