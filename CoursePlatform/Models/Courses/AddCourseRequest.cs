using FluentValidation;

namespace CoursesPlatform.Models.Courses
{
    public class AddCourseRequest
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }
    }

    public class AddCourseRequestValidator : AbstractValidator<AddCourseRequest>
    {
        public AddCourseRequestValidator()
        {
            RuleFor(x => x.Title).Must(x => x.Length >= 10 && x.Length <= 70)
                                 .WithMessage("Title must be between 10 and 70 symbols !");
            RuleFor(x => x.Description).Must(x => x.Length >= 15 && x.Length <= 150)
                                       .WithMessage("Description must be between 15 and 150 symbols !");
            RuleFor(x => x.ImageUrl).Must(x => x.Length >= 7)
                                    .WithMessage("Image URL must contain at least 7 symbols !");
        }
    }
}
