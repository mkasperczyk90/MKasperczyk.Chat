using MKasperczyk.Chat.Api.DAL;

namespace MKasperczyk.Chat.Api.Repositories
{
    public interface IConnectionRepository
    {
        Task<UserConnection?> GetConnection(int userId);
        Task AddConnection(int userId, string connectionId, string userAgent);
        void Disconnect(int userId);
    }
}
