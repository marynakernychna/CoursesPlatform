using FluentValidation;
using System;
using System.Collections.Generic;
namespace CoursesPlatform.Models.Users
{
    public class StudentsOnPageRequest
    {
        public int PageNumber { get; set; }

        public int ElementsOnPage { get; set; }
    }

    public class StudentsOnPageRequestValidator : AbstractValidator<StudentsOnPageRequest>
    {
        public StudentsOnPageRequestValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1)
                                      .WithMessage("Invalid page number !");
            RuleFor(x => x.ElementsOnPage).GreaterThanOrEqualTo(1)
                                          .WithMessage("Invalid number of elements on page !");
        }
    }
}
