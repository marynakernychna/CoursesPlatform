using System.Threading.Tasks;

namespace CoursesPlatform.Interfaces
{
    public interface ITemplateHelper
    {
        Task<string> GetTemplateHtmlAsStringAsync<T>(string viewName, T model);
    }
}
