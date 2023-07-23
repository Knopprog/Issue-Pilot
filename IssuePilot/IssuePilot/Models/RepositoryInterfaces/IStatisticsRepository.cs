using IssuePilot.Models.ViewModels.Statistics;
using System.Threading.Tasks;

namespace IssuePilot.Models.RepositoryInterfaces
{
    public interface IStatisticsRepository
    {
        Task<StatisticsDetailsViewModel> GetProjectStatisticsDataAsync(int projectId);
        Task<StatisticsCalendarViewModel> GetCalendarDataAsync(int projectId);
    }
}
