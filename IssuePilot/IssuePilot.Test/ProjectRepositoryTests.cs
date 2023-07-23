using IssuePilot.Models.DBModels;
using IssuePilot.Models.Repositorys;
using IssuePilot.Models.ViewModels;
using IssuePilot.Test.TestData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace IssuePilot.Test
{
    public class ProjectRepositoryTests : InitDbWithData
    {
        /*
         * CreateProjectAsync test cases
         */
        [Fact]
        public async Task CreateProjectAsyncTest_HasTitleBeenCreated()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRepository projectRepository = new ProjectRepository(context);
            User user = context.Users.First();
            ProjectUpdateViewModel modelToSave = new ProjectUpdateViewModel() { Title = "SaveTestProjectTitle"};
            // Act
            var (resultModel, resultProject) = await projectRepository.CreateProjectAsync(modelToSave, user);
            // Assert
            Assert.Equal(resultModel.Title, modelToSave.Title);
        }
        [Fact]
        public async Task CreateProjectAsyncTest_HasDescriptionBeenCreated()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRepository projectRepository = new ProjectRepository(context);
            User user = context.Users.First();
            ProjectUpdateViewModel modelToSave = new ProjectUpdateViewModel() { Title = "SaveTestProjectTitle", Description = "A Test Project from a Test!" };
            // Act
            var (resultModel, resultProject) = await projectRepository.CreateProjectAsync(modelToSave, user);
            // Assert
            Assert.Equal(resultModel.Description, modelToSave.Description);
        }

        [Fact]
        public async Task CreateProjectAsyncTest_IsCurrentDateAdded()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRepository projectRepository = new ProjectRepository(context);
            User user = context.Users.First();
            ProjectUpdateViewModel modelToSave = new ProjectUpdateViewModel() { Title = "SaveTestProjectTitle"};
            DateTime currentTime = DateTime.Now;
            // Act
            var (resultModel, resultProject) = await projectRepository.CreateProjectAsync(modelToSave, user);
            // Assert
            Assert.True(resultProject.CreateDate > currentTime);
        }

        [Fact]
        public async Task CreateProjectAsyncTest_ProjectCreatorIsSet()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRepository projectRepository = new ProjectRepository(context);
            var listOfTicketCategories = context.TicketCategories.ToList();
            User user = context.Users.First();
            ProjectUpdateViewModel modelToSave = new ProjectUpdateViewModel() { Title = "SaveTestProjectTitle" };
            // Act
            var (resultModel, resultProject) = await projectRepository.CreateProjectAsync(modelToSave, user);
            // Assert
            Assert.NotNull(user.Projects.FirstOrDefault(r => r.Id == resultProject.Id));
        }

        [Fact]
        public async Task CreateProjectAsyncTest_DefaultTicketCategoriesHaveBeenCreated()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRepository projectRepository = new ProjectRepository(context);
            var listOfTicketCategories = context.TicketCategories.ToList();
            User user = context.Users.First();
            ProjectUpdateViewModel modelToSave = new ProjectUpdateViewModel() { Title = "SaveTestProjectTitle" };
            const int DEFAULT_CATEGORY_AMOUNT= 5;
            // Act
            var (resultModel, resultProject) = await projectRepository.CreateProjectAsync(modelToSave, user);
            // Assert
            var listOfCategories = context.TicketCategories.Where(p => p.Project.Id == resultProject.Id);
            Assert.Equal(DEFAULT_CATEGORY_AMOUNT, listOfCategories.Count());
        }

        [Fact]
        public async Task CreateProjectAsyncTest_ValidateTitleAlreadyExists()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRepository projectRepository = new ProjectRepository(context);
            User user = context.Users.First();
            ProjectUpdateViewModel modelToSave = new ProjectUpdateViewModel() { Title = "SaveTestProjectTitle", Description = "A Test Project from a Test!" };
            // Act
            var (resultModel, resultProject) = await projectRepository.CreateProjectAsync(modelToSave, user);
            var (resultModelFail, resultProjectFail) = await projectRepository.CreateProjectAsync(new ProjectUpdateViewModel { Title = "TestProjekt1" }, user);
            // Assert
            Assert.True(resultModelFail.TitleExists);
        }

        /*
         * CreateTicketCategoryAsync test cases
         */
        [Fact]
        public async Task CreateTicketCategoryAsyncTest_IsNewTicketCategoryCreated()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRepository projectRepository = new ProjectRepository(context);
            Project project = context.Projects.FirstOrDefault();
            int seededCategoriesCount = ListOfTicketCategories().Count();
            // Act
            await projectRepository.CreateTicketCategoryAsync("TestBug", project);
            // Assert
            Assert.Equal(seededCategoriesCount + 1, context.TicketCategories.Count());
        }

        /*
         * UpdateProjectAsync test cases
         */
        [Fact]
        public async Task UpdateProjectAsyncTest_UpdatesTitle()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRepository projectRepository = new ProjectRepository(context);
            Project project = await context.Projects.FirstOrDefaultAsync();
            string newTitle = "new Title";
            ProjectUpdateViewModel modelSucceedTitleAndDescription = new ProjectUpdateViewModel() { Id = project.Id, Title = newTitle, Description = project.Description };
            // Act
            var updateSucceedTitleAndDescription = await projectRepository.UpdateProjectAsync(modelSucceedTitleAndDescription);
            // Assert
            Assert.Equal(newTitle, updateSucceedTitleAndDescription.Title);
        }
        [Fact]
        public async Task UpdateProjectAsyncTest_UpdatesDescription()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRepository projectRepository = new ProjectRepository(context);
            Project project = await context.Projects.FirstOrDefaultAsync();
            string description = "new description";
            ProjectUpdateViewModel modelSucceedTitleAndDescription = new ProjectUpdateViewModel() { Id = project.Id, Title = project.Title, Description = description };
            // Act
            var updateSucceedTitleAndDescription = await projectRepository.UpdateProjectAsync(modelSucceedTitleAndDescription);
            // Assert
            Assert.Equal(description, updateSucceedTitleAndDescription.Description);
        }

        [Fact]
        public async Task UpdateProjectAsyncTest_ValidateTitleAlreadyExists()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRepository projectRepository = new ProjectRepository(context);
            List<Project> listOfProjects = ListOfProjects();
            Project project = await context.Projects.FirstOrDefaultAsync();
            ProjectUpdateViewModel modelFailed = new ProjectUpdateViewModel() { Id = project.Id, Title = listOfProjects[1].Title };
            // Act
            var updateFailed = await projectRepository.UpdateProjectAsync(modelFailed);
            // Assert
            Assert.True(updateFailed.TitleExists);
        }

        /*
         * UpdateTicketCategoriesAsync test cases
         */
        [Fact]
        public async Task UpdateTicketCategoriesAsyncTest_CategoryNameUpdated()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRepository projectRepository = new ProjectRepository(context);
            TicketCategory ticketCategory = await context.TicketCategories.FirstOrDefaultAsync();
            string categoryName = "updated";
            // Act
            await projectRepository.UpdateTicketCategoryAsync(ticketCategory.Id, categoryName);
            // Assert
            var dbCategory = await context.TicketCategories.FindAsync(ticketCategory.Id);
            Assert.Equal(categoryName, dbCategory.Name);
        }

        /*
         * DeleteProjectAsync test cases
         */
        [Fact]
        public async Task DeleteProjectAsyncTest_ProjectDeleted()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            ProjectRepository projectRepository = new ProjectRepository(context, ticketRepository);
            List<Project> listOfProjects = await context.Projects.ToListAsync();
            // Act
            await projectRepository.DeleteProjectAsync(listOfProjects.First().Id);
            // Assert
            Assert.Null(await context.Projects.FindAsync(listOfProjects.First().Id));
        }

        [Fact]
        public async Task DeleteProjectAsyncTest_ArgumentNullExceptionIfProjectsIsNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            ProjectRepository projectRepository = new ProjectRepository(context, ticketRepository);
            const int NONEXISTENT_ID = 300;
            // Act
            async Task actual () => await projectRepository.DeleteProjectAsync(NONEXISTENT_ID);
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(actual);
        }
        /*
         * DeleteTicketCategoryAsync test cases
         */
        [Fact]
        public async Task DeleteTicketCategoryAsyncTest_CategoryDeleted()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            ProjectRepository projectRepository = new ProjectRepository(context, ticketRepository);
            var category = await context.TicketCategories.FirstAsync();
            // Act
            await projectRepository.DeleteTicketCategoryAsync(category.Id);
            // Assert
            Assert.Null(await context.TicketCategories.FindAsync(category.Id));
        }
    }
}
