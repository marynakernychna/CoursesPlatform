using FluentValidation;
using System;

namespace CoursesPlatform.Models.Courses
{
    public class EnrollCourseRequest
    {
        public DateTime StartDate { get; set; }

        public int CourseId { get; set; }
    }

    public class EnrolCourseRequestValidator : AbstractValidator<EnrollCourseRequest>
    {
        public EnrolCourseRequestValidator()
        {
            RuleFor(x => x.CourseId).GreaterThanOrEqualTo(1)
                                    .WithMessage("Id must be greater than or equal to 1 !");

        }
    }
}
