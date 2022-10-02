using MKasperczyk.Chat.Api.Features.Auth;

namespace MKasperczyk.Chat.Api.Services
{
    public interface IAuthService
    {
        bool VerifyPassword(SecurityTokenRequest tokenRequest, string? passwordFromDb);
        string GetToken(int idUser, string userName);
    }
}
