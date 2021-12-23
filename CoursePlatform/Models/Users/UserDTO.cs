using FluentValidation;
using System;

namespace CoursesPlatform.Models.Users
{
    public class UserDTO
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public DateTime Birthday { get; set; }

        public string Email { get; set; }
    }

    public class UserDTOValidator : AbstractValidator<UserDTO>
    {
        public UserDTOValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Surname).NotNull().NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
        }
    }
}
