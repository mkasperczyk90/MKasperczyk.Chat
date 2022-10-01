using MKasperczyk.Chat.Api.DAL;

namespace MKasperczyk.Chat.Api.Repositories
{
    public interface IMessageRepository
    {
        Task<ChatChanel?> GetOrCreateChanelWithRecipientsAsync(IEnumerable<int> recipientIds);
        Task AddMessageAsync(string message, int chanelId, int senderId);
    }
}
