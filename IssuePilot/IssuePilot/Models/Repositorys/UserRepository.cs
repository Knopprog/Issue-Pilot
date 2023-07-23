using IssuePilot.Data;
using IssuePilot.Models.RepositoryInterfaces;
using IssuePilot.Models.ViewModels;
using IssuePilot.Models.DBModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssuePilot.Models.Repositorys
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IDashboardRepository _dashboardRepository;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public UserRepository(ApplicationDbContext context, UserManager<User> userManager, IDashboardRepository dashboardRepository)
        {
            _context = context;
            _userManager = userManager;
            _dashboardRepository = dashboardRepository;
        }
        
        public string GenerateUserName(string firstname, string surname)
        {
            string generatedUserName = "";
            string extraNumber = "";
            int count = 0;
            do
            {
                count++;
                extraNumber = count.ToString().PadLeft(4, '0');
                generatedUserName = firstname + surname + extraNumber;

            } while (_context.Users.Any(t => t.UserName == generatedUserName && count < 3001));
            return generatedUserName;
        }

        public async Task<(User, IdentityResult)> AddUserAsync(User newUser, string password)
        {
            newUser.CreateDate = DateTime.Now;
            newUser.UserName = GenerateUserName(newUser.Firstname, newUser.Surname);
            IdentityResult result = await _userManager.CreateAsync(newUser, password);
            User returnUser = await _context.Users.FirstOrDefaultAsync(c => c.UserName == newUser.UserName);
            return (returnUser, result);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.Where(c => c.IsDeleted == false).ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<IdentityResult> UpdateUserAsync(AdministrationEditViewModel editUserModel)
        {
            User databaseUser = await _context.Users.FindAsync(editUserModel.Id);
            if (editUserModel.Email != databaseUser.Email)
            {
                if(editUserModel.Email == null)
                {
                    databaseUser.NormalizedEmail = editUserModel.Email;
                }
                else
                {
                    databaseUser.NormalizedEmail = editUserModel.Email.ToUpper();
                }
                
                databaseUser.Email = editUserModel.Email;

            }
            
            if(databaseUser.Firstname != editUserModel.Firstname || databaseUser.Surname != editUserModel.Surname)
            {
                databaseUser.Firstname = editUserModel.Firstname;
                databaseUser.Surname = editUserModel.Surname;
                await _userManager.SetUserNameAsync(databaseUser, GenerateUserName(editUserModel.Firstname, editUserModel.Surname));
                await _userManager.UpdateNormalizedUserNameAsync(databaseUser);
            }
            var result = await _userManager.UpdateAsync(databaseUser);
            return result;
        }
        public async Task<IdentityResult> UpdatePasswordAsync(AdministrationChangePasswordViewModel changePasswordModel)
        {
            User databaseUser = await _context.Users.FindAsync(changePasswordModel.Id);
            var token = await _userManager.GeneratePasswordResetTokenAsync(databaseUser);
            IdentityResult result = await _userManager.ResetPasswordAsync(databaseUser, token, changePasswordModel.Password);
            return (result);
        }

        public async Task DeleteUserAsync(string userId)
        {
            User databaseUser = await _context.Users.FindAsync(userId);
            databaseUser.Email = null;
            databaseUser.CreateDate = DateTime.MinValue;
            databaseUser.NormalizedEmail = null;
            databaseUser.Firstname = null;
            databaseUser.Surname = null;
            databaseUser.PasswordHash = null;
            databaseUser.PhoneNumber = null;
            databaseUser.UserName = databaseUser.Id;
            databaseUser.NormalizedUserName = databaseUser.UserName.ToUpper();
            databaseUser.IsDeleted = true;

            _context.Users.Update(databaseUser);
            await _context.SaveChangesAsync();
            await _dashboardRepository.DeleteAllEntriesOfUserAsync(userId);

        }
    }
}
