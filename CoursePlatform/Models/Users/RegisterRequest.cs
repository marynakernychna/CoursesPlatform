using FluentValidation;
using System;

namespace CoursesPlatform.Models.Users
{
    public class RegisterRequest
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public DateTime Birthday { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }

    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Name).Must(x => x.Length > 0 && x.Length <= 30)
                                .WithMessage("Name must be between 0 and 30 symbols !")
                                .Matches(@"(?=.*[a-zA-Z])")
                                .WithMessage("Name must contains only letters !");
            RuleFor(x => x.Surname).Must(x => x.Length > 0 && x.Length <= 50)
                                   .WithMessage("Surname must be between 0 and 50 symbols !")
                                   .Matches(@"(?=.*[a-zA-Z])")
                                   .WithMessage("Name must contains only letters !");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Invalid email !");
            RuleFor(x => x.Password).Must(x => x.Length >= 6 && x.Length <= 20)
                                    .WithMessage("Password must be between 6 and 20 symbols !")
                                    .Matches(@"(?=.*[A-Z])")
                                    .WithMessage("Password must contain at least one uppercase letter !")
                                    .Matches(@"(?=.*?[0-9])")
                                    .WithMessage("Password must contain at least one digit !")
                                    .Matches(@"(?=.*?[!@#\$&*~])")
                                    .WithMessage("Password must contain at least one Special character !");
        }
    }
}
