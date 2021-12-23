using FluentValidation;

namespace CoursesPlatform.Models.Users
{
    public class AuthenticateRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }

    public class AuthenticateRequestValidator : AbstractValidator<AuthenticateRequest>
    {
        public AuthenticateRequestValidator()
        {
            RuleFor(x => x.Email).EmailAddress().WithMessage("Invalid email !");
            RuleFor(x => x.Password).Must(x => x.Length >= 6 && x.Length <= 20)
                                    .WithMessage("Password must be between 6 and 20 symbols !")
                                    .Matches(@"(?=.*[A-Z])")
                                    .WithMessage("Password must contain at least 1 lowercase letter !")
                                    .Matches(@"(?=.*?[0-9])")
                                    .WithMessage("Password must contain at least 1 digit !")
                                    .Matches(@"(?=.*?[!@#\$&*~])")
                                    .WithMessage("Password must contain at least 1 Special character !");
        }
    }
}
