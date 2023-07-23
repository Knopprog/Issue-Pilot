using IssuePilot.Helper;
using IssuePilot.Models.RepositoryInterfaces;
using IssuePilot.Models.ViewModels.Statistics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IssuePilot.Controllers
{
    [Authorize(Roles = "Admin,Projektmanager,Benutzer")]
    public class StatisticsController : ProjectAccessController
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IStatisticsRepository _statisticsRepository;

        public StatisticsController(IProjectRepository projectRepository, IStatisticsRepository statisticsRepository, IProjectRoleRepository projectRoleRepository) : base(projectRoleRepository, projectRepository)
        {
            _projectRepository = projectRepository;
            _statisticsRepository = statisticsRepository;
        }

        // GET: Statistics
        public async Task<ActionResult> Index(string sortOrder, int? pageNumber, string currentFilter, string searchString)
        {
            return View(await GetProjectsAsync(sortOrder, pageNumber, currentFilter, searchString));
        }

        // GET: Statistics/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var project = await _projectRepository.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            if (await ProjectAccessAsync(id))
            {
                return View(await _statisticsRepository.GetProjectStatisticsDataAsync(id));
            }
            else
            {
                return RedirectToAction(nameof(NoProjectAccess));
            } 
        }
        // GET: Statistics/Calendar/5
        public async Task<ActionResult> Calendar(int id)
        {
            var project = await _projectRepository.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            if (await ProjectAccessAsync(id))
            {
                return View(await _statisticsRepository.GetCalendarDataAsync(id));
            }
            else
            {
                return RedirectToAction(nameof(NoProjectAccess));
            }
        }

        // GET: Statistics/CompareWith/5
        public async Task<ActionResult> CompareWith(int id, string sortOrder, int? pageNumber, string currentFilter, string searchString)
        {
            var projectToRemove = await _projectRepository.GetProjectByIdAsync(id);
            if (projectToRemove == null)
            {
                return NotFound();
            }
            if (await ProjectAccessAsync(id))
            {
                var projects = await GetProjectsAsync(sortOrder, pageNumber, currentFilter, searchString);
                projects.Remove(projectToRemove);
                CompareWithViewModel model = new CompareWithViewModel()
                {
                    Projects = projects,
                    FirstId = id,
                    FirstProjectTitle = projectToRemove.Title
                };
                return View(model);
            }
            else
            {
                return RedirectToAction(nameof(NoProjectAccess));
            }
        }

        // POST: Statistics/Comparison
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Comparison([FromForm] CompareWithViewModel compareWithViewModel)
        {
            var project1 = await _projectRepository.GetProjectByIdAsync(compareWithViewModel.FirstId);
            var project2 = await _projectRepository.GetProjectByIdAsync(compareWithViewModel.SecondId);
            if (project1 == null || project2 == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (await ProjectAccessAsync(project1.Id) && await ProjectAccessAsync(project2.Id))
                {
                    ComparisonViewModel viewModel = new ComparisonViewModel
                    {
                        StatisticsModelFirst = await _statisticsRepository.GetProjectStatisticsDataAsync(compareWithViewModel.FirstId),
                        StatisticsModelSecond = await _statisticsRepository.GetProjectStatisticsDataAsync(compareWithViewModel.SecondId)
                    };
                    return View(viewModel);
                }
                return RedirectToAction(nameof(NoProjectAccess));
            }
            return NotFound();

        }
    }
}
