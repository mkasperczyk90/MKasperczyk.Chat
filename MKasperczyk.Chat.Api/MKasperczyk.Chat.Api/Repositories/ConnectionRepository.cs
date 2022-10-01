using Microsoft.EntityFrameworkCore;
using MKasperczyk.Chat.Api.DAL;
using System.Linq;
using System.Threading.Channels;

namespace MKasperczyk.Chat.Api.Repositories
{
    public class ConnectionRepository : IConnectionRepository
    {
        private ChatContext _context;
        public ConnectionRepository(ChatContext chatContext)
        {
            _context = chatContext;
        }

        public async Task<UserConnection?> GetConnection(int userId)
        {
            return await _context.Connections.FirstOrDefaultAsync(con => 
                con.ConnectedUserId == userId &&
                con.Connected == true);
        }

        public async Task AddConnection(int userId,string connectionId, string userAgent)
        {
            await _context.Connections.AddAsync(new UserConnection
            {
                ConnectionID = connectionId,
                ConnectedUserId = userId,
                ConnectionAt = DateTime.UtcNow, // TODO: datetimeprovider
                UserAgent = userAgent,
                Connected = true
            });
            _context.SaveChanges();
        }
        public void Disconnect(int userId)
        {
            var connections = _context.Connections.Where(con =>
                con.ConnectedUserId == userId &&
                con.Connected == true);

            foreach (var connection in connections)
            {
                connection.Connected = false;
                connection.ConnectionAt = DateTime.UtcNow; // TODO: datetimeprovider
            }

            _context.SaveChanges();
        }

    }
}
