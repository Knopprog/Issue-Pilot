using IssuePilot.Helper;
using IssuePilot.Models.RepositoryInterfaces;
using IssuePilot.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IssuePilot.Controllers
{
    [Authorize(Roles = "Admin,Projektmanager,Benutzer")]
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardController(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }
        public async Task<IActionResult> Index(int? pageNumber)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var listOfNews = await _dashboardRepository.GetNewsByUserIdAsync(currentUserId);
            await _dashboardRepository.UpdateSeenStatusesAsync(listOfNews);

            int pageSize = 15;
            return View(await PaginatedList<DashboardIndexViewModel>.CreateFromListAsync(listOfNews, pageNumber ?? 1, pageSize));
        }
    }
}
