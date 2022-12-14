using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MKasperczyk.Chat.Api.DAL;
using MKasperczyk.Chat.Api.Services;

namespace MKasperczyk.Chat.Api.Features.Auth
{
    public class SecurityTokenHandler
    {
        public async static Task<IResult> Handle(IDbContextFactory<ChatContext> dbContextFactory, IAuthService authService, IValidator<SecurityTokenRequest> validator, SecurityTokenRequest tokenRequest)
        {
            var validationResult = validator.Validate(tokenRequest);
            if (!validationResult.IsValid)
            {
                return Results.Json(new
                {
                    Success = false,
                    Message = validationResult?.Errors?.FirstOrDefault()?.ErrorMessage
                });
            }

            using var dbContext = dbContextFactory.CreateDbContext();

            var loggedInUser = await dbContext.Users.FirstOrDefaultAsync(user => user.Username == tokenRequest.UserName);
            if (loggedInUser == null)
            {
                return Results.Unauthorized();
            }

            bool isCorrect = authService.VerifyPassword(tokenRequest, loggedInUser.Password);
            if (isCorrect)
            {
                return Results.Unauthorized();
            }

            string token = authService.GetToken(loggedInUser.Id, tokenRequest.UserName);

            return Results.Json(new
            {
                Id = loggedInUser.Id,
                Token = token,
                User = loggedInUser.Username,
                Avatar = loggedInUser.Avatar,
                Success = true
            });
        }
    }
}
