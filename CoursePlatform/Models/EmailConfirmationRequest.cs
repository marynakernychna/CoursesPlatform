using FluentValidation;

namespace CoursesPlatform.Models
{
    public class EmailConfirmationRequest
    {
        public string Token { get; set; }

        public string Email { get; set; }
    }

    public class EmailConfirmationRequestValidator : AbstractValidator<EmailConfirmationRequest>
    {
        public EmailConfirmationRequestValidator()
        {
            RuleFor(x => x.Token).NotEmpty().NotNull().WithMessage("Token is null !");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Invalid email !");
        }
    }
}
