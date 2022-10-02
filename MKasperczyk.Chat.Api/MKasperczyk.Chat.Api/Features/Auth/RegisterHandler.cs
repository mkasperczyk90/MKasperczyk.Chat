using FluentValidation;
using Microsoft.AspNetCore.Identity;
using MKasperczyk.Chat.Api.Repositories;
using MKasperczyk.Chat.Api.Services;

namespace MKasperczyk.Chat.Api.Features.Auth
{
    public class RegisterHandler
    {
        public async static Task<IResult> Handle(IUnitOfWork unitOfWork, IValidator<RegisterRequest> validator, IAuthService authService, RegisterRequest registerInfo)
        {
            var validationResult = validator.Validate(registerInfo);

            if (!validationResult.IsValid)
            {
                return Results.BadRequest();
            }
            var user = await unitOfWork.UserRepository.GetUserAsync(registerInfo.UserName);
            if (user != null)
            {
                return Results.Json(new
                {
                    message = "User exists",
                    success = false
                });
            }

            PasswordHasher<string> passwordHasher = new PasswordHasher<string>();
            var hashedPassword = passwordHasher.HashPassword(registerInfo.UserName, registerInfo.Password);

            await unitOfWork.UserRepository.AddUserAsync(registerInfo.UserName, hashedPassword);
            unitOfWork.Save();

            var newUser = await unitOfWork.UserRepository.GetUserAsync(registerInfo.UserName);
            if (newUser == null)
            {
                return Results.Json(new
                {
                    message = "Problem with saving user.",
                    success = false
                });
            }
            string token = authService.GetToken(newUser.Id, registerInfo.UserName);
            return Results.Json(new
            {
                Id = newUser.Id,
                Token = token,
                User = newUser.Username,
                Success = true,
            });
        }
    }
}
