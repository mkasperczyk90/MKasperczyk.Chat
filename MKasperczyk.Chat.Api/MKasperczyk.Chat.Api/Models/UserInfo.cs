namespace MKasperczyk.Chat.Api.Models
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public DateTime? LastConnection { get; set; }
        public bool CurrentlyLogin { get; set; }
        public byte[]? Avatar { get; set; }
    }
}
