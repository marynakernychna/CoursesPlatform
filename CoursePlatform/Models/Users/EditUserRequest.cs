using FluentValidation;

namespace CoursesPlatform.Models.Users
{
    public class EditUserRequest
    {
        public UserDTO User { get; set; }

        public string CurrentUserEmail { get; set; }
    }

    public class EditUserRequestValidator : AbstractValidator<EditUserRequest>
    {
        public EditUserRequestValidator()
        {
            RuleFor(x => x.User).NotEmpty().NotNull().WithMessage("User is null !");
            RuleFor(x => x.CurrentUserEmail).EmailAddress().WithMessage("Invalid email !");
        }
    }
}
