using IssuePilot.Models.DBModels;
using IssuePilot.Models.Repositorys;
using IssuePilot.Models.ViewModels;
using IssuePilot.Test.TestData;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace IssuePilot.Test
{

    public class UserRepositoryTests : InitDbWithData
    {
        /*
         * The following method(s) are not tested because they use the methods of the identity system (.net core 3.1).
         * UpdatePasswordAsync()
         */

        /* AddUserAsync test cases */
        [Fact]
        public async Task AddUserAsyncTest_IsUserAdded()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            UserManager<User> userManager = new UserManager<User>(new UserStore<User>(context), null, new PasswordHasher<User>(null), null, null, null, null, null, null);
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            UserRepository userRepository = new UserRepository(context, userManager, dashboardRepository);
            User userToCreate = new User
            {
                Firstname = "Firstname",
                Surname = "Surname",
                Email = "Email"
            };
            // Act
            (User, IdentityResult) result = await userRepository.AddUserAsync(userToCreate, "");
            // Assert
            Assert.True(context.Users.FindAsync(result.Item1.Id) != null);
        }
        [Fact]
        public async Task AddUserAsyncTest_ValidationSucceeded()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            UserManager<User> userManager = new UserManager<User>(new UserStore<User>(context), null, new PasswordHasher<User>(null), null, null, null, null, null, null);
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            UserRepository userRepository = new UserRepository(context, userManager, dashboardRepository);
            User userToCreate = new User
            {
                Firstname = "Firstname",
                Surname = "Surname",
                Email = "Email"
            };
            // Act
            (User, IdentityResult) result = await userRepository.AddUserAsync(userToCreate, "");
            // Assert
            Assert.True(result.Item2.Succeeded);
        }

        /* GenerateUserName test cases */
        [Fact]
        public void GenerateUserNameTest_RetrunsValidName()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            UserRepository userRepository = new UserRepository(context);
            // Act
            var username = userRepository.GenerateUserName("Test", "er");
            // Assert
            Assert.Contains("Tester", username);
        }
        /*
         * GetUserByIdAsync test cases
         */
        [Fact]
        public async Task GetUserByIdAsyncTest_ReturnsNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            UserRepository userRepository = new UserRepository(context);
            // Act
            var user = await userRepository.GetUserByIdAsync(null);
            // Assert
            Assert.Null(user);
        }

        /*
         * UpdateUserAsync test cases
         */
        [Fact]
        public async Task UpdateUserAsyncTest_EmailIsUpdated()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            UserManager<User> userManager = new UserManager<User>(new UserStore<User>(context),null,null,null,null,null,null,null,null);
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            UserRepository userRepository = new UserRepository(context, userManager, dashboardRepository);
            List<User> listOfUsers = ListOfUsers();
            User userToUpdate = listOfUsers[1];
            AdministrationEditViewModel model = new AdministrationEditViewModel
            {
                Id = userToUpdate.Id,
                Firstname = "UpdatedFirstname",
                Surname = "UpdatedSurname",
                Email = "UpdatedEmail"
            };
            // Act
            await userRepository.UpdateUserAsync(model);
            User updatedUser = await context.Users.FindAsync(userToUpdate.Id);
            // Assert
            Assert.Equal(updatedUser.Email, model.Email);
        }

        [Fact]
        public async Task UpdateUserAsyncTest_FirstNameIsUpdated()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            UserManager<User> userManager = new UserManager<User>(new UserStore<User>(context), null, null, null, null, null, null, null, null);
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            UserRepository userRepository = new UserRepository(context, userManager, dashboardRepository);
            List<User> listOfUsers = ListOfUsers();
            User userToUpdate = listOfUsers[1];
            AdministrationEditViewModel model = new AdministrationEditViewModel
            {
                Id = userToUpdate.Id,
                Firstname = "UpdatedFirstname",
                Surname = "UpdatedSurname",
                Email = "UpdatedEmail"
            };

            // Act
            await userRepository.UpdateUserAsync(model);
            User updatedUser = await context.Users.FindAsync(userToUpdate.Id);
            // Assert
            Assert.Equal(updatedUser.Firstname, model.Firstname);
        }
        [Fact]
        public async Task UpdateUserAsyncTest_SurnameIsUpdated()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            UserManager<User> userManager = new UserManager<User>(new UserStore<User>(context), null, null, null, null, null, null, null, null);
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            UserRepository userRepository = new UserRepository(context, userManager, dashboardRepository);
            List<User> listOfUsers = ListOfUsers();
            User userToUpdate = listOfUsers[1];
            AdministrationEditViewModel model = new AdministrationEditViewModel
            {
                Id = userToUpdate.Id,
                Firstname = "UpdatedFirstname",
                Surname = "UpdatedSurname",
                Email = "UpdatedEmail"
            };

            // Act
            await userRepository.UpdateUserAsync(model);
            User updatedUser = await context.Users.FindAsync(userToUpdate.Id);
            // Assert
            Assert.Equal(updatedUser.Surname, model.Surname);
        }

        [Fact]
        public async Task UpdateUserAsyncTest_UpdateEmptyEmail()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            UserManager<User> userManager = new UserManager<User>(new UserStore<User>(context), null, null, null, null, null, null, null, null);
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            UserRepository userRepository = new UserRepository(context, userManager, dashboardRepository);
            List<User> listOfUsers = ListOfUsers();
            User userToUpdate = listOfUsers[1];
            AdministrationEditViewModel model = new AdministrationEditViewModel
            {
                Id = userToUpdate.Id,
                Email = ""
            };
            // Act
            await userRepository.UpdateUserAsync(model);
            User updatedUser = await context.Users.FindAsync(userToUpdate.Id);
            // Assert
            Assert.Equal(updatedUser.Email, model.Email);
        }

        [Fact]
        public async Task UpdateUserAsyncTest_NullReferenceExceptionIfModelIsNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            UserRepository userRepository = new UserRepository(context);
            // Act
            async Task actual() => await userRepository.UpdateUserAsync(null);
            // Assert
            await Assert.ThrowsAsync<NullReferenceException>(actual);
        }

        [Fact]
        public async Task UpdateUserAsyncTest_NullReferenceExceptionIfUserIsNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            UserRepository userRepository = new UserRepository(context);
            // Act
            async Task actual() => await userRepository.UpdateUserAsync(new AdministrationEditViewModel { Id = "" });
            // Assert
            await Assert.ThrowsAsync<NullReferenceException>(actual);
        }

        // *
        // * DeleteUserAsync test cases
        // */
        [Fact]
        public async Task DeleteUserAsyncTest_NameIsId()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            UserManager<User> userManager = new UserManager<User>(new UserStore<User>(context), null, null, null, null, null, null, null, null);
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            UserRepository userRepository = new UserRepository(context, userManager, dashboardRepository);
            const string USER_ID = "2301D884-221A-4E7D-B509-0113DCC043E0";
            User user = context.Users.Find(USER_ID);
            // Act
            await userRepository.DeleteUserAsync(user.Id);
            // Assert
            Assert.Contains(USER_ID, user.UserName);
        }
        [Fact]
        public async Task DeleteUserAsyncTest_EmailIsNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            UserManager<User> userManager = new UserManager<User>(new UserStore<User>(context), null, null, null, null, null, null, null, null);
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            UserRepository userRepository = new UserRepository(context, userManager, dashboardRepository);
            const string USER_ID = "2301D884-221A-4E7D-B509-0113DCC043E0";
            User user = context.Users.Find(USER_ID);
            // Act
            await userRepository.DeleteUserAsync(user.Id);
            // Assert
            Assert.Null(user.Email);
        }
        [Fact]
        public async Task DeleteUserAsyncTest_FirstNameIsNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            UserManager<User> userManager = new UserManager<User>(new UserStore<User>(context), null, null, null, null, null, null, null, null);
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            UserRepository userRepository = new UserRepository(context, userManager, dashboardRepository);
            const string USER_ID = "2301D884-221A-4E7D-B509-0113DCC043E0";
            User user = context.Users.Find(USER_ID);
            // Act
            await userRepository.DeleteUserAsync(user.Id);
            Assert.Null(user.Firstname);
        }
        [Fact]
        public async Task DeleteUserAsyncTest_SurNameIsNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            UserManager<User> userManager = new UserManager<User>(new UserStore<User>(context), null, null, null, null, null, null, null, null);
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            UserRepository userRepository = new UserRepository(context, userManager, dashboardRepository);
            const string USER_ID = "2301D884-221A-4E7D-B509-0113DCC043E0";
            User user = context.Users.Find(USER_ID);
            // Act
            await userRepository.DeleteUserAsync(user.Id);
            // Assert
            Assert.Null(user.Surname);

        }
        [Fact]
        public async Task DeleteUserAsyncTest_PasswordHashIsNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            UserManager<User> userManager = new UserManager<User>(new UserStore<User>(context), null, null, null, null, null, null, null, null);
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            UserRepository userRepository = new UserRepository(context, userManager, dashboardRepository);
            const string USER_ID = "2301D884-221A-4E7D-B509-0113DCC043E0";
            User user = context.Users.Find(USER_ID);
            // Act
            await userRepository.DeleteUserAsync(user.Id);
            // Assert
            Assert.Null(user.PasswordHash);
        }
        [Fact]
        public async Task DeleteUserAsyncTest_IsDeletedSetToTrue()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            UserManager<User> userManager = new UserManager<User>(new UserStore<User>(context), null, null, null, null, null, null, null, null);
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            UserRepository userRepository = new UserRepository(context, userManager, dashboardRepository);
            const string USER_ID = "2301D884-221A-4E7D-B509-0113DCC043E0";
            User user = context.Users.Find(USER_ID);
            // Act
            await userRepository.DeleteUserAsync(user.Id);
            // Assert
            Assert.True(user.IsDeleted);
        }

        [Fact]
        public async Task DeleteUserAsyncTest_ExceptionUserIdIsNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            UserManager<User> userManager = new UserManager<User>(new UserStore<User>(context), null, null, null, null, null, null, null, null);
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            UserRepository userRepository = new UserRepository(context, userManager, dashboardRepository);
            const string USER_ID = "2301D884-221A-4E7D-B509-0113DCC043E0";
            User user = context.Users.Find(USER_ID);
            // Act
            await userRepository.DeleteUserAsync(user.Id);
            async Task actual() => await userRepository.DeleteUserAsync(null);
            // Assert
            await Assert.ThrowsAsync<NullReferenceException>(actual);
        }

        [Fact]
        public async Task DeleteUserAsyncTest_NullReferenceExceptionIfUserIsNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            UserManager<User> userManager = new UserManager<User>(new UserStore<User>(context), null, null, null, null, null, null, null, null);
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            UserRepository userRepository = new UserRepository(context, userManager, dashboardRepository);
            const string USER_ID = "2301D884-221A-4E7D-B509-0113DCC043E0";
            User user = context.Users.Find(USER_ID);
            // Act
            await userRepository.DeleteUserAsync(user.Id);
            async Task actual() => await userRepository.DeleteUserAsync("");
            // Assert
            await Assert.ThrowsAsync<NullReferenceException>(actual);
        }
        // *
        // * GetAllUsersAsync test cases
        // */
        [Fact]
        public async Task GetAllUsersAsyncAndDeleteUserAsyncTest_ReturnsOnlyNonDeletedUsers()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            UserManager<User> userManager = new UserManager<User>(new UserStore<User>(context), null, null, null, null, null, null, null, null);
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            UserRepository userRepository = new UserRepository(context, userManager, dashboardRepository);
            const string USER_ID = "2301D884-221A-4E7D-B509-0113DCC043E0";
            User user = context.Users.Find(USER_ID);
            // Act
            await userRepository.DeleteUserAsync(user.Id);
            var users = await userRepository.GetAllUsersAsync();
            // Assert
            Assert.DoesNotContain<User>(user, users);
        }

    }
}
