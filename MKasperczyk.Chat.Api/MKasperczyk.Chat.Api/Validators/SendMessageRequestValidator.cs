using FluentValidation;
using FluentValidation.Validators;
using MKasperczyk.Chat.Api.Features.Messages;

namespace MKasperczyk.Chat.Api.Validators
{
    public class SendMessageRequestValidator : AbstractValidator<SendMessageRequest>
    {
        public SendMessageRequestValidator()
        {
            RuleFor(user => user.Message).NotEmpty();
            RuleFor(user => user.Sender).GreaterThan(0);
            RuleFor(user => user.Recipients).Must(to => to.Any()).WithMessage("Recipients should be greater then 0")
                .ForEach(to =>
                {
                    to.GreaterThan(0).WithMessage("Recipients should have correct Id number");
                });
        }
    }
}
