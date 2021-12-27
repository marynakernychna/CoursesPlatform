using FluentValidation;
namespace CoursesPlatform.Models.Users
{
    public class SearchStudentsRequest
    {
        public string SearchText { get; set; }

        public StudentsOnPageRequest StudentsOnPageRequest { get; set; }
    }

    public class SearchStudentsRequestValidator : AbstractValidator<SearchStudentsRequest>
    {
        public SearchStudentsRequestValidator()
        {
            RuleFor(x => x.SearchText).NotEmpty().NotNull()
                                      .WithMessage("Search text is null!");
        }
    }
}
