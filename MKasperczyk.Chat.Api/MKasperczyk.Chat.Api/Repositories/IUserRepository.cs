using MKasperczyk.Chat.Api.DAL;

namespace MKasperczyk.Chat.Api.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<IEnumerable<User>> GetUsersWithConnectionAsync();
        Task<User?> GetUserAsync(int id);
        Task<User?> GetUserAsync(string userName);
        Task AddUserAsync(string userName, string password);
    }
}
