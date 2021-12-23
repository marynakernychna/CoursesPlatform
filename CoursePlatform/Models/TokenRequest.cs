using FluentValidation;

namespace CoursesPlatform.Models
{
    public class TokenRequest
    {
        public string Token { get; set; }
    }

    public class TokenRequestValidator : AbstractValidator<TokenRequest>
    {
        public TokenRequestValidator()
        {
            RuleFor(x => x.Token).NotNull().NotEmpty()
                                 .WithMessage("Token is null or empty !");
        }
    }
}
