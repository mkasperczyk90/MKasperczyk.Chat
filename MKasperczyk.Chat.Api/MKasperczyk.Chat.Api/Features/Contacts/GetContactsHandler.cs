using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MKasperczyk.Chat.Api.DAL;
using MKasperczyk.Chat.Api.Models;
using MKasperczyk.Chat.Api.Repositories;

namespace MKasperczyk.Chat.Api.Features.Contacts
{
    public class GetContactsHandler
    {
        public async static Task<IResult> Handle(IUnitOfWork unitOfWork, int userId)
        {
            var users = await unitOfWork.UserRepository.GetUsersWithConnectionAsync();
            var contacts = users
                .Where(u => u.Id != userId)
                .Select(u =>
                {
                    var connection = u?.Connections?.OrderByDescending(c => c.ConnectionAt)?.FirstOrDefault();

                    return new UserInfo()
                    {
                        Id = u.Id,
                        UserName = u.Username,
                        LastConnection = connection?.ConnectionAt,
                        CurrentlyLogin = connection != null ? connection.Connected : false,
                        Avatar = u.Avatar
                    };
                })
                .ToList();

            return Results.Json(contacts);
        }
    }
}
