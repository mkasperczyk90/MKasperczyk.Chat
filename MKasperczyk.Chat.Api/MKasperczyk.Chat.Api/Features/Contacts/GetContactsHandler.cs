using MKasperczyk.Chat.Api.Repositories;

namespace MKasperczyk.Chat.Api.Features.Contacts
{
    public class GetContactsHandler
    {
        public async static Task<IResult> Handle(IUnitOfWork unitOfWork, int userId)
        {
            var users = await unitOfWork.UserRepository.GetUsersWithConnectionAsync();
            var contacts = users
                .Where(user => user.Id != userId)
                .Select(user =>
                {
                    var connection = user?.Connections?.OrderByDescending(c => c.ConnectionAt)?.FirstOrDefault();

                    return new GetContactsResponse(
                        user.Id,
                        user.Username,
                        connection?.ConnectionAt,
                        connection != null ? connection.Connected : false,
                        user.Avatar);
                })
                .ToList();

            return Results.Json(contacts);
        }
    }
}
