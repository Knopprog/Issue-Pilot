using IssuePilot.Helper;
using IssuePilot.Models.DBModels;
using IssuePilot.Models.RepositoryInterfaces;
using IssuePilot.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IssuePilot.Controllers
{
    [Authorize(Roles = "Admin,Projektmanager,Benutzer")]
    public class AdministrationController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IAspNetRoleRepository _aspNetRoleRepository;
        private readonly IProjectRoleRepository _projectRoleRepository;
        public AdministrationController(IProjectRoleRepository projectRoleRepository, IUserRepository userRepository, IAspNetRoleRepository aspNetRoleRepository)
        {
            _userRepository = userRepository;
            _aspNetRoleRepository = aspNetRoleRepository;
            _projectRoleRepository = projectRoleRepository;

        }

        // GET: Administration
        public async Task<IActionResult> Index(string sortOrder, int? pageNumber, string currentFilter, string searchString)
        {
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["EmailSortParm"] = sortOrder == "Email" ? "email_desc" : "Email";
            ViewData["UsernameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            var users = await _userRepository.GetAllUsersAsync();
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToUpper();
                users = users.Where(s => s.UserName.ToUpper().Contains(searchString)).ToList();
            }
            ViewData["CurrentSort"] = sortOrder;
            switch (sortOrder)
            {
                case "Date":
                    users = users.OrderBy(s => s.CreateDate).ToList();
                    break;
                case "date_desc":
                    users = users.OrderByDescending(s => s.CreateDate).ToList();
                    break;
                case "Email":
                    users = users.OrderBy(s => s.Email).ToList();
                    break;
                case "email_desc":
                    users = users.OrderByDescending(s => s.Email).ToList();
                    break;
                case "name_desc":
                    users = users.OrderByDescending(s => s.UserName).ToList();
                    break;
                default:
                    users = users.OrderBy(s => s.UserName).ToList();
                    break;
            }
            int pageSize = 15;
            return View(await PaginatedList<User>.CreateFromListAsync(users, pageNumber ?? 1, pageSize));

        }

        // GET: Administration/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Administration/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] AdministrationCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Role == "Benutzer" || model.Role == "Projektmanager" || model.Role == "Admin")
                {
                    User user = new User { Email = model.Email, Firstname = model.Firstname, Surname = model.Surname };
                    (User, IdentityResult) userResult = await _userRepository.AddUserAsync(user, model.Password);
                    if (userResult.Item2.Succeeded)
                    {
                        IdentityResult roleResult = await _aspNetRoleRepository.AddUserToIdentityRoleAsync(userResult.Item1, model.Role);
                        return RedirectToAction(nameof(Index));
                    }
                    // Password Validation RequireDigit, RequiredLength=6, RequireUppercase, RequireLowercase
                    foreach (IdentityError error in userResult.Item2.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        // GET: Administration/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            User user = await _userRepository.GetUserByIdAsync(id);
            if (user == null || user.IsDeleted)
            {
                return NotFound();
            }
            string role = await _aspNetRoleRepository.GetRoleOfUserByIdAsync(user);
            AdministrationEditViewModel viewData = new AdministrationEditViewModel { UserName = user.UserName, CurrentRole = role, Id = user.Id, Firstname = user.Firstname, Surname = user.Surname, Email = user.Email };
            return View(viewData);
        }

        // POST: Administration/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit([FromForm] AdministrationEditViewModel model)
        {
            User user = await _userRepository.GetUserByIdAsync(model.Id);
            if (user == null || user.IsDeleted)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var originalName = user.UserName;

                var result = await _userRepository.UpdateUserAsync(model);
                string dbRole = await _aspNetRoleRepository.GetRoleOfUserByIdAsync(user);
                if (model.Role != dbRole && model.Role != "keine Änderung" && (model.Role == "Benutzer" || model.Role == "Projektmanager" || model.Role == "Admin"))
                {
                    await _aspNetRoleRepository.RemoveUserFromIdentityRoleAsync(user, model.CurrentRole);
                    await _aspNetRoleRepository.AddUserToIdentityRoleAsync(user, model.Role);
                }

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    model.UserName = originalName;
                    return View(model);
                }
            }
            else
            {
                model.UserName = user.UserName;
                return View(model);
            }
        }

        // GET: Administration/ChangePassword/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangePassword(string id)
        {
            User user = await _userRepository.GetUserByIdAsync(id);
            if (user == null || user.IsDeleted)
            {
                return NotFound();
            }
            string role = await _aspNetRoleRepository.GetRoleOfUserByIdAsync(user);
            AdministrationChangePasswordViewModel viewData = new AdministrationChangePasswordViewModel { UserName = user.UserName, Id = user.Id, CurrentRole = role };
            return View(viewData);
        }

        // POST: Administration/ChangePassword/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangePassword([FromForm] AdministrationChangePasswordViewModel model)
        {
            User user = await _userRepository.GetUserByIdAsync(model.Id);
            if (user == null || user.IsDeleted)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                // update user data
                IdentityResult identityResult = await _userRepository.UpdatePasswordAsync(model);
                if (identityResult.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                //Password Validation RequireDigit, RequiredLength = 6, RequireUppercase, RequireLowercase
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            model.UserName = user.UserName;
            string role = await _aspNetRoleRepository.GetRoleOfUserByIdAsync(user);
            model.CurrentRole = role;
            return View(model);
        }

        // GET: Administration/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            User user = await _userRepository.GetUserByIdAsync(id);
            if (user == null || user.IsDeleted)
            {
                return NotFound();
            }
            string role = await _aspNetRoleRepository.GetRoleOfUserByIdAsync(user);
            AdministrationDeleteViewModel viewData = new AdministrationDeleteViewModel { Role = role, Id = user.Id, Firstname = user.Firstname, Surname = user.Surname, Email = user.Email, UserName = user.UserName };
            return View(viewData);
        }

        // POST: Administration/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            User user = await _userRepository.GetUserByIdAsync(id);
            if (user == null || user.IsDeleted)
            {
                return NotFound();
            }
            var projects = await _projectRoleRepository.GetProjectsOfUserByIdAsync(user.Id);
            foreach (var project in projects)
            {
                await _projectRoleRepository.RemoveMemberFromProjectAsync(project.Id, user.Id);
            }

            string role = await _aspNetRoleRepository.GetRoleOfUserByIdAsync(user);
            if (role != "")
            {
                await _aspNetRoleRepository.RemoveUserFromIdentityRoleAsync(user, role);
            }

            await _userRepository.DeleteUserAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
