
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Reflection.Metadata;

namespace MKasperczyk.Chat.Api.DAL
{
    public class ChatContext: DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Message> Messages => Set<Message>(); 
        public DbSet<ChatChanel> Chanels => Set<ChatChanel>(); 
        public DbSet<ChanelRecipients> ChanelUsers => Set<ChanelRecipients>();
        public DbSet<UserConnection> Connections => Set<UserConnection>();


        public ChatContext(DbContextOptions<ChatContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChatChanel>()
                .HasMany(chanel => chanel.Messages)
                .WithOne(msg => msg.Chanel)
                .HasForeignKey(chanel => chanel.ChanelId);

            modelBuilder.Entity<ChatChanel>()
                .HasMany(chanel => chanel.Recipients);

            modelBuilder.Entity<User>()
                .HasMany(user => user.Connections)
                .WithOne(con => con.ConnectedUser);
        }
    }
}
