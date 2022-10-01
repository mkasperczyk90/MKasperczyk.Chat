namespace MKasperczyk.Chat.Api.Features.Messages
{
    public record SendMessageRequest(int Sender, int[] Recipients, string Message);
}
