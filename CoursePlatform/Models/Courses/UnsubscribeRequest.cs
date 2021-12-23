using FluentValidation;

namespace CoursesPlatform.Models.Courses
{
    public class UnsubscribeRequest
    {
        public int CourseId { get; set; }
    }

    public class UnsubscribeRequestValidator : AbstractValidator<UnsubscribeRequest>
    {
        public UnsubscribeRequestValidator()
        {
            RuleFor(x => x.CourseId).GreaterThanOrEqualTo(1)
                                    .WithMessage("Id must be greater than or equal to 1 !");

        }
    }
}
