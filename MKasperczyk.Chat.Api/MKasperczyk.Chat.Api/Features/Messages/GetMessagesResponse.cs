namespace MKasperczyk.Chat.Api.Features.Messages
{
    public record GetMessagesResponse(int MessageId, string? Message, DateTime SendAt, string Type);
}
