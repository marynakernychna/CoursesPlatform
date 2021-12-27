using FluentValidation;
using System;

namespace CoursesPlatform.Models.Users
{
    public class EditProfileRequest
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string Birthday { get; set; } // for nullable

        public string CurrentEmail { get; set; }
    }

    public class EditProfileRequestValidator : AbstractValidator<EditProfileRequest>
    {
        public EditProfileRequestValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Surname).NotNull().NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.CurrentEmail).EmailAddress();
        }
    }
}
