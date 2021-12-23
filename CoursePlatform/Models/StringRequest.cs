using FluentValidation;

namespace CoursesPlatform.Models
{
    public class StringRequest
    {
        public string Value { get; set; }
    }
    public class StringRequestValidator : AbstractValidator<StringRequest>
    {
        public StringRequestValidator()
        {
            RuleFor(x => x.Value).NotNull().NotEmpty()
                                 .WithMessage("Value is null or empty !");
        }
    }
}
