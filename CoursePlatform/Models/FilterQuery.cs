using FluentValidation;

namespace CoursesPlatform.Models.Courses
{
    public class FilterQuery
    {
        public enum SortDirection_enum { ASC, DESC };
        public enum SortBy_enum { DATE, TITLE, NAME, SURNAME, REGISTEREDDATE, AGE };

        public int PageNumber { get; set; }

        public int ElementsOnPage { get; set; }

        public SortDirection_enum SortDirection { get; set; }

        public SortBy_enum SortBy { get; set; }
    }

    public class FilterQueryValidator : AbstractValidator<FilterQuery>
    {
        public FilterQueryValidator()
        {
            RuleFor(x => x.SortDirection).IsInEnum()
                                         .WithMessage("Invalid sort direction type !");
            RuleFor(x => x.SortBy).IsInEnum()
                                  .WithMessage("Invalid sort by type !");
            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1)
                                      .WithMessage("Invalid page number !");
            RuleFor(x => x.ElementsOnPage).GreaterThanOrEqualTo(1)
                                          .WithMessage("Invalid number of elements on page !");
        }
    }
}
