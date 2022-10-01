using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MKasperczyk.Chat.Api.DAL;
using MKasperczyk.Chat.Api.Models;
using MKasperczyk.Chat.Api.Repositories;
using System.IO.Compression;

namespace MKasperczyk.Chat.Api.Features.Contacts
{
    public class AvatarHandler
    {
        public async static Task<IResult> Handle(IUnitOfWork unitOfWork, HttpRequest request)
        {
            int? id = GetUserId(request.HttpContext);

            using (var memoryStream = new MemoryStream())
            {
                await request.Form.Files[0].CopyToAsync(memoryStream);

                // Upload the file if less than 10 kb
                if (memoryStream.Length < 10240)
                {
                    var user = await unitOfWork.UserRepository.GetUserAsync(id.Value);

                    if(user == null)
                    {
                        return Results.Json(new
                        {
                            Success = false,
                            Message = "The file is too large."
                        });
                    }

                    user.Avatar = memoryStream.ToArray();
                    unitOfWork.Save();
                }
                else
                {
                    return Results.Json(new
                    {
                        Success = false,
                        Message = "The file is too large."
                    });
                }
            }
            return Results.Ok();
        }

        private static int? GetUserId(HttpContext context)
        {
            var id = context.User?.Claims?.FirstOrDefault(claim => claim.Type == "Id")?.Value;
            return int.TryParse(id, out var number) ? number : null;
        }
    }
}
