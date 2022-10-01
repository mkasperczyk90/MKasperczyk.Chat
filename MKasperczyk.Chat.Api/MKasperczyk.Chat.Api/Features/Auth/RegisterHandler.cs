using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MKasperczyk.Chat.Api.DAL;
using MKasperczyk.Chat.Api.Models;
using MKasperczyk.Chat.Api.Repositories;
using MKasperczyk.Chat.Api.Services;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

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

            user = await unitOfWork.UserRepository.GetUserAsync(registerInfo.UserName);
            string token = authService.GetToken(user.Id, registerInfo.UserName);
            return Results.Json(new
            {
                Id = user.Id,
                Token = token,
                User = user.Username,
                Success = true,
                Avatar = user.Avatar
            });
        }
    }
}
