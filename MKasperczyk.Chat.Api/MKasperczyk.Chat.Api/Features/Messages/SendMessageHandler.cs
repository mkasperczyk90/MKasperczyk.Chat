using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MKasperczyk.Chat.Api.DAL;
using MKasperczyk.Chat.Api.Models;
using MKasperczyk.Chat.Api.Repositories;

namespace MKasperczyk.Chat.Api.Features.Messages
{
    public class SendMessageHandler
    {
        public async static Task<IResult> Handle(
            IUnitOfWork unitOfWork,
            IDbContextFactory<ChatContext> dbContextFactory,
            IValidator<SendMessageRequest> validator,
            SendMessageRequest model)
        {
            // TODO: Secure request. Do not know that recipients are correct + sender are correct.
            var validationResult = validator.Validate(model);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest();
            }

            using var dbContext = dbContextFactory.CreateDbContext();

            List<int> userInChanel = model.Recipients.ToList();
            userInChanel.Add(model.Sender);

            User? sender = await unitOfWork.UserRepository
                .GetUserAsync(model.Sender);

            ChatChanel? chanel = await unitOfWork.MessageRepository
                .GetOrCreateChanelWithRecipientsAsync(userInChanel);

            unitOfWork.Save();

            if (sender == null || chanel == null)
            {
                return Results.Json(new
                {
                    success = false,
                    message = "User can not be found"
                });
            }

            await unitOfWork.MessageRepository.AddMessageAsync(model.Message, chanel.Id, sender.Id);
            unitOfWork.Save();

            return Results.Json(new
            {
                success = true,
                message = "User Added"
            });
        }
    }
}
