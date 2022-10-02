using Microsoft.EntityFrameworkCore;
using MKasperczyk.Chat.Api.DAL;

namespace MKasperczyk.Chat.Api.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private ChatContext _context;

        public MessageRepository(ChatContext chatContext) => _context = chatContext;

        public async Task<IEnumerable<User>> GetUsersAsync() => await _context.Users.ToListAsync();

        public async Task AddMessageAsync(string message, int chanelId, int senderId) =>
            await _context.Messages.AddAsync(new Message
            {
                Content = message,
                SenderId = senderId,
                ChanelId = chanelId,
                SendAt = DateTime.UtcNow
            });

        public async Task<ChatChanel?> GetOrCreateChanelWithRecipientsAsync(IEnumerable<int> recipientIds)
        {
            var chanel = await _context.Chanels
                .FirstOrDefaultAsync(chanel => chanel.Recipients.All(a => recipientIds.Contains(a.UserId)));

            if (chanel == null)
            {
                chanel = new ChatChanel()
                {
                    Name = "Default",
                };
                await _context.Chanels.AddAsync(chanel);

                List<ChanelRecipients> recipients = new List<ChanelRecipients>();
                foreach (var recipientId in recipientIds)
                {
                    recipients.Add(new ChanelRecipients()
                    {
                        Chanel = chanel,
                        UserId = recipientId
                    });
                }
                await _context.ChanelUsers.AddRangeAsync(recipients);
            }

            return chanel;
        }

    }
}
