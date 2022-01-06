using CoursesPlatform.Models.Courses;
using System.Linq;

namespace CoursesPlatform.Interfaces.Commands
{
    public interface IGeneralCommands
    {
        IQueryable<T> GetElementsOnPage<T>(FilterQuery request, IQueryable<T> query) where T : class;
    }
}
