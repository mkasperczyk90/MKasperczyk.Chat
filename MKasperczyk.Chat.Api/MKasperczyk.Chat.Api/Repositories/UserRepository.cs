using Microsoft.EntityFrameworkCore;
using MKasperczyk.Chat.Api.DAL;

namespace MKasperczyk.Chat.Api.Repositories
{
    public class UserRepository: IUserRepository
    {
        private ChatContext _context;
        public UserRepository(ChatContext chatContext)
        {
            _context = chatContext;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<IEnumerable<User>> GetUsersWithConnectionAsync()
        {
            return await _context.Users.Include(u => u.Connections).ToListAsync();
        }

        public async Task AddUserAsync(string userName, string password)
        {
            await _context.Users.AddAsync(new User
            {
                Username = userName,
                Password = password
            });
        }

        public async Task<User?> GetUserAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
        }
        public async Task<User?> GetUserAsync(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Username == userName);
        }
    }
}
