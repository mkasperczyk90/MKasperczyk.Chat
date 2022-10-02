using Microsoft.AspNetCore.SignalR;
using MKasperczyk.Chat.Api.DAL;
using MKasperczyk.Chat.Api.Repositories;

namespace MKasperczyk.Chat.Api.Hubs
{
    public class ChatHub : Hub
    {
        private IUnitOfWork _unitOfWork { get; set; }
        
        public ChatHub(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task SendMessage(int receiver, string message)
        {
            int? id = GetUserId();

            var connection = await GetConnectionIdAsync(receiver);
            
            if(connection != null)
            {
                await Clients.Client(connection.ConnectionID).SendAsync("ReceiveMessage", new
                {
                    Message = message,
                    Sender = id,
                    Receiver = receiver
                });
            }
        }

        public async override Task OnConnectedAsync()
        {
            int? id = GetUserId();
            
            if(id.HasValue && Context != null)
            {
                await _unitOfWork.ConnectionRepository.AddConnection(
                    id.Value,
                    Context.ConnectionId,
                    Context.GetHttpContext()?.Request?.Headers["User-Agent"]);

                await Clients.AllExcept(Context.ConnectionId).SendAsync("statusChanged", new // TODO: do not use anonymous obj
                {
                    Online = true,
                    User = id,
                    When = DateTime.UtcNow // TODO: should use datetime provider
                });
            }

            await base.OnConnectedAsync();
        }

        public async override Task OnDisconnectedAsync(Exception? exception)
        {
            int? id = GetUserId();

            if (id.HasValue)
            {
                _unitOfWork.ConnectionRepository.Disconnect(id.Value);

                await Clients.AllExcept(Context.ConnectionId).SendAsync("statusChanged", new // TODO: do not use anonymous obj
                {
                    Online = false,
                    User = id,
                    When = DateTime.UtcNow // TODO: should use datetime provider
                });
            } 

            await base.OnDisconnectedAsync(exception);
        }

        private int? GetUserId()
        {
            var id = Context.User?.Claims?.FirstOrDefault(claim => claim.Type == "Id")?.Value;
            return int.TryParse(id, out var number) ? number : null;
        }

        private async Task<UserConnection?> GetConnectionIdAsync(int userId)
        {
            var user = await _unitOfWork.UserRepository.GetUserAsync(userId);
            if (user == null)
            {
                await Clients.Caller.SendAsync("showErrorMessage", "Could not find that user.", CancellationToken.None);
            }
            else
            {
                var connection = await _unitOfWork.ConnectionRepository.GetConnection(user.Id);
                if (connection == null)
                {
                    await Clients.Caller.SendAsync("showErrorMessage", "The user is no longer connected.", CancellationToken.None);

                    return null;
                }
                return connection;
            }
            return null;
        }
    }
}
