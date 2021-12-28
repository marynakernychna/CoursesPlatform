using CoursesPlatform.Models.Courses;
using FluentValidation;
using System;
using System.Collections.Generic;
namespace CoursesPlatform.Models.Users
{
    public class StudentsOnPageRequest
    {
        public enum SearchBy_enum { NAME, SURNAME };

        public string SearchText { get; set; }

        public List<SearchBy_enum> SearchBy { get; set; }

        public FilterQuery FilterQuery { get; set; }

    }

    public class StudentsOnPageRequestValidator : AbstractValidator<StudentsOnPageRequest>
    {
        public StudentsOnPageRequestValidator()
        {
        }
    }
}
