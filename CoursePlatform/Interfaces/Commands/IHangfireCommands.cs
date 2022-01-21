using CoursesPlatform.EntityFramework.Models;

namespace CoursesPlatform.Interfaces.Commands
{
    public interface IHangfireCommands
    {
        void CreateScheduleHangfireJob(ScheduleHangfireJob scheduleHangfireJob);
    }
}
