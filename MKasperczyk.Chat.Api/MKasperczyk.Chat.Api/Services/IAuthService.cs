using MKasperczyk.Chat.Api.Models;
using System.ComponentModel;

namespace MKasperczyk.Chat.Api.Services
{
    public interface IAuthService
    {
        bool VerifyPassword(TokenRequest tokenRequest, string passwordFromDb);
        string GetToken(int idUser, string userName);
    }
}
