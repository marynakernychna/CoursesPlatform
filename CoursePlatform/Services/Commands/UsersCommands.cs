using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.Interfaces.Commands;
using CoursesPlatform.Models.Courses;
using CoursesPlatform.Models.Users;
using System.Collections.Generic;
using System.Linq;

namespace CoursesPlatform.Services.Commands
{
    public class UsersCommands : IUsersCommands
    {
        private readonly ICoursesCommands coursesCommands;

        public UsersCommands(ICoursesCommands coursesCommands)
        {
            this.coursesCommands = coursesCommands;
        }

        #region form

        public List<StudentDTO> FormStudentsList(IQueryable<User> students)
        {
            List<StudentDTO> studentsDTOs = new List<StudentDTO>();

            foreach (var student in students)
            {
                var subscriptions = coursesCommands.GetUserSubscriptions(student.Id);

                studentsDTOs.Add(new StudentDTO
                {
                    Name = student.Name,
                    Surname = student.Surname,
                    Email = student.Email,
                    Birthday = student.Birthday,
                    IsEmailConfirmed = student.EmailConfirmed,
                    Subscriptions = subscriptions
                });
            }

            return studentsDTOs;
        }

        #endregion

        #region change

        public void DeleteUserSubscribes(string userId)
        {
            List<CourseDTO> subscriptions = coursesCommands.GetUserSubscriptions(userId);

            foreach (var item in subscriptions)
            {
                coursesCommands.UnsubscribeFromCourse(userId, item.Id);
            }
        }


        #endregion

        public List<StudentDTO> GetStudentsOnPage(FilterQuery request, IQueryable<User> students)
        {
            var studentsDtos = FormStudentsList(students);

            return studentsDtos.Skip((request.PageNumber - 1) * request.ElementsOnPage)
                               .Take(request.ElementsOnPage)
                               .ToList();
        }
    }
}
