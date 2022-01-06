using CoursesPlatform.Interfaces.Commands;
using CoursesPlatform.Models.Courses;
using System.Linq;

namespace CoursesPlatform.Services.Commands
{
    public class GeneralCommands : IGeneralCommands
    {
        public GeneralCommands() { }

        public IQueryable<T> GetElementsOnPage<T>(FilterQuery request, IQueryable<T> query) where T : class
        {
            return query.Skip((request.PageNumber - 1) * request.ElementsOnPage)
                        .Take(request.ElementsOnPage);
        }
    }
}
