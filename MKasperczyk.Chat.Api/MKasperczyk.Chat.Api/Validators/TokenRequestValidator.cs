using FluentValidation;
using MKasperczyk.Chat.Api.Models;

namespace MKasperczyk.Chat.Api.Validators
{
    public class TokenRequestValidator : AbstractValidator<TokenRequest>
    {
        public TokenRequestValidator()
        {
            RuleFor(m => m.UserName).MinimumLength(4);
            RuleFor(m => m.Password).MinimumLength(8);
        }
    }
}
