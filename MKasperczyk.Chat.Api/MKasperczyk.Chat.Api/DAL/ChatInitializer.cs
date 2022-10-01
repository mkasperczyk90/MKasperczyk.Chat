using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MKasperczyk.Chat.Api.Models;

namespace MKasperczyk.Chat.Api.DAL
{
    public class ChatInitializer
    {
        public async static Task Seed(ChatContext context)
        {
            //context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var adminUser = await context.Users.FirstOrDefaultAsync(u => u.Username == "Geek0n");

            if (adminUser == null)
            {
                PasswordHasher<string> passwordHasher = new PasswordHasher<string>();

                await context.Users.AddAsync(new User()
                {
                    Username = "Geek0n",
                    Password = passwordHasher.HashPassword("Geek0n", "Geek0n")
                });

                context.SaveChanges();
            }
        }
    }
}
