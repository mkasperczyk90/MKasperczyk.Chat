using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MKasperczyk.Chat.Api.DAL
{
    [Table("UserConnections")]
    public class UserConnection
    {
        [Key]
        public string ConnectionID { get; set; }
        public int ConnectedUserId { get; set; }
        public string? UserAgent { get; set; }
        public bool Connected { get; set; }
        public User ConnectedUser { get; set; }
        public DateTime ConnectionAt { get; set; }
    }
}
