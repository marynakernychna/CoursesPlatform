using CoursesPlatform.Models.Courses;
using FluentValidation;
using System;
using System.Collections.Generic;
namespace CoursesPlatform.Models.Users
{
    public class OnPageRequest
    {
        public string SearchText { get; set; }

        public FilterQuery FilterQuery { get; set; }
    }

    public class StudentsOnPageRequestValidator : AbstractValidator<OnPageRequest>
    {
        public StudentsOnPageRequestValidator()
        {
        }
    }
}
