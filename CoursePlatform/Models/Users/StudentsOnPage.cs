using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesPlatform.Models.Users
{
    public class StudentsOnPage
    {
        public int TotalCount { get; set; }

        public List<StudentDTO> Students { get; set; }
    }
}
