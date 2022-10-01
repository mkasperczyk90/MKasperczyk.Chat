using MKasperczyk.Chat.Api.DAL;

namespace MKasperczyk.Chat.Api.Repositories
{
    public interface IUnitOfWork: IDisposable
    {
        IUserRepository UserRepository { get; }
        IMessageRepository MessageRepository { get; }
        IConnectionRepository ConnectionRepository { get; }
        void Save();
    }
}
