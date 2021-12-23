using CoursesPlatform.Models.Courses;
using System;
using System.Collections.Generic;

namespace CoursesPlatform.Models.Users
{
    public class StudentDTO
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public DateTime Birthday { get; set; }

        public bool IsEmailConfirmed { get; set; }

        public int SubscriptionsCount { get; set; }

        public List<CourseDTO> Subscriptions { get; set; }
    }
}
