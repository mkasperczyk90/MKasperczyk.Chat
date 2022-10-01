namespace MKasperczyk.Chat.Api.Features.Contacts
{
    public record GetContactsResponse(int Id, string? UserName, DateTime? LastConnection, bool CurrentlyLogin, byte[]? Avatar);
}
