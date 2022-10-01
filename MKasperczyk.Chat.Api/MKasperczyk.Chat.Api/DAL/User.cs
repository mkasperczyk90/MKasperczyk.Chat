using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MKasperczyk.Chat.Api.DAL
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public byte[]? Avatar { get; set; }
        public ICollection<UserConnection> Connections { get; set; }
    }
}
