using FluentValidation;
using MKasperczyk.Chat.Api.Features.Auth;

namespace MKasperczyk.Chat.Api.Validators
{
    public class TokenRequestValidator : AbstractValidator<SecurityTokenRequest>
    {
        public TokenRequestValidator()
        {
            RuleFor(m => m.UserName).MinimumLength(4);
            RuleFor(m => m.Password).MinimumLength(8);
        }
    }
}
