using IssuePilot.Models.DBModels;
using IssuePilot.Models.Repositorys;
using IssuePilot.Test.TestData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace IssuePilot.Test
{
    public class ProjectRoleRepositoryTests : InitDbWithData
    {
        /*
         * AddProjectMemberEntryAsync test cases
         */
        [Fact]
        public async Task AddProjectMemberEntryAsyncTest_UserIsAssignedToProject()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRepository = new ProjectRoleRepository(context);
            User user = context.Users.Last();
            var roleId = ListOfProjectRoles().First().Id;
            Project project = context.Projects.First();
            // Act
            await projectRepository.AddProjectMemberEntryAsync(user, roleId, project);
            var entry = await context.ProjectMemberEntries.FirstOrDefaultAsync(e => e.FK_ProjectId == project.Id && e.FK_UserId == user.Id);
            // Assert
            Assert.NotNull(entry);
        }

        [Fact]
        public async Task AddProjectMemberEntryAsyncTest_ThrowInvalidOperationExceptionIfUserIdIsEmpty()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            var roleId = context.ProjectRoles.First().Id;
            var project = context.Projects.First();
            var user = context.Users.First();
            user.Id = "";
            // Act
            async Task actual() => await projectRoleRepository.AddProjectMemberEntryAsync(user, roleId, project);
            //Assert
            await Assert.ThrowsAsync<InvalidOperationException>(actual);
        }


        [Fact]
        public async Task AddProjectMemberEntryAsyncTest_ThrowInvalidOperationExceptionIfAlreadyProjectMember()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            var project = context.Projects.Last();
            var user = context.Users.First();
            await projectRoleRepository.AddProjectMemberEntryAsync(user, ProjectRoleId.Member, project);
            // Act
            async Task actual() => await projectRoleRepository.AddProjectMemberEntryAsync(user, ProjectRoleId.Owner, project);
            //Assert
            await Assert.ThrowsAsync<InvalidOperationException>(actual);
        }
        [Fact]
        public async Task AddProjectMemberEntryAsyncTest_ThrowInvalidOperationExceptionIfUserIsNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            var project = context.Projects.Last();
            var user = context.Users.First();
            // Act
            async Task actual() => await projectRoleRepository.AddProjectMemberEntryAsync(null, ProjectRoleId.Owner, project);
            //Assert
            await Assert.ThrowsAsync<InvalidOperationException>(actual);
        }

        /*
         * GetListOfProjectsOfUserByIdAsync test cases
         */
        [Fact]
        public async Task GetProjectsOfUserByIdAsyncTest_ReturnsSingleResult()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            var userId = ListOfUsers()[1].Id;
            // Act
            var result = await projectRoleRepository.GetProjectsOfUserByIdAsync(userId);
            // Assert
            Assert.Single(result);
        }

        [Fact]
        public async Task GetProjectsOfUserByIdAsyncTest_ReturnsNoResult()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            const string USER_ID  = "2301D884-221A-4E7D-B509-0113DCC043E3";
            // Act
            var result = await projectRoleRepository.GetProjectsOfUserByIdAsync(USER_ID);
            // Assert
            Assert.Empty(result);
        }
        /*
         * GetProjectRoleOfUserAsync test cases
         */
        [Fact]
        public async Task GetProjectRoleOfUserAsyncTest_ReturnsCorrectRole()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            var userId = ListOfUsers()[1].Id;
            var projectId = ListOfProjects()[1].Id;
            var expectedEntry = await context.ProjectMemberEntries.FirstOrDefaultAsync(r => r.FK_ProjectId == projectId && r.FK_UserId == userId);
            // Act
            var projectRole = await projectRoleRepository.GetProjectRoleOfUserAsync(userId, projectId);
            // Assert
            Assert.Equal(expectedEntry.ProjectRole.Id, projectRole.Id);
        }

        [Fact]
        public async Task GetProjectRoleOfUserAsyncTest_NullReferenceExceptionUserIdIsNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            var projectId = ListOfProjects()[1].Id;
            // Act
            async Task actual() => await projectRoleRepository.GetProjectRoleOfUserAsync(null, projectId);
            //Assert
            await Assert.ThrowsAsync<NullReferenceException>(actual);
        }

        [Fact]
        public async Task GetProjectRoleOfUserAsyncTest_ExceptionProjectIsNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            var userId = ListOfUsers()[1].Id;
            const int PROJECT_ID = 300;
            // Act
            async Task actual() => await projectRoleRepository.GetProjectRoleOfUserAsync(userId,PROJECT_ID);
            //Assert
            await Assert.ThrowsAsync<NullReferenceException>(actual);
        }

        /*
         * GetMembersOfProjectByIdAsync test cases
         */
        [Fact]
        public async Task GetMembersOfProjectByIdAsyncTest_ReturnMembers()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            const int MEMBER_COUNT_OF_PROJECT = 3;
            // Act
            var resultWithUsers = await projectRoleRepository.GetMembersOfProjectByIdAsync(ListOfProjects()[1].Id);
            // Assert
            Assert.Equal(MEMBER_COUNT_OF_PROJECT, resultWithUsers.Count);
        }

        [Fact]
        public async Task GetMembersOfProjectByIdAsyncTest_ReturnEmptyUserListIfProjectHasZeroUsers()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            // Act
            List<User> resultWithoutUsers = await projectRoleRepository.GetMembersOfProjectByIdAsync(ListOfProjects()[2].Id);
            // Assert
            Assert.Empty(resultWithoutUsers);
        }

        [Fact]
        public async Task GetMembersOfProjectByIdAsyncTest_ReturnEmptyUserListIfProjectIsNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            const int PROJECT_ID = 300;
            // Act
            List<User> resultWithoutUsers = await projectRoleRepository.GetMembersOfProjectByIdAsync(PROJECT_ID);
            //Assert
            Assert.Empty(resultWithoutUsers);
        }

        /*
         * GetAllNonMembersOfProjectAsync test cases
         */
        [Fact]
        public async Task GetAllNonMembersOfProjectAsyncTest_ReturnsCorrectNumberOfUsers()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            var projectId = ListOfProjects()[1].Id;
            var listOfMembers = await context.ProjectMemberEntries.Where(r => r.FK_ProjectId == projectId).ToListAsync();
            var listOfUsers = await context.Users.ToListAsync();
            // Act
            var listOfNonMembersOfProject = await projectRoleRepository.GetAllNonMembersOfProjectAsync(projectId);
            // Assert
            Assert.Equal(listOfUsers.Count - listOfMembers.Count, listOfNonMembersOfProject.Count);
        }
        /*
         * RemoveMemberFromProjectAsync test cases
         */
        [Fact]
        public async Task RemoveMemberFromProjectAsyncTest_MemberIsRemoved()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            var entry = await context.ProjectMemberEntries.FirstOrDefaultAsync();
            // Act
            await projectRoleRepository.RemoveMemberFromProjectAsync(entry.FK_ProjectId, entry.FK_UserId);
            // Assert
            Assert.Null(await context.ProjectMemberEntries.FirstOrDefaultAsync(r => r.FK_ProjectId == entry.FK_ProjectId && r.FK_UserId == entry.FK_UserId));
        }

        [Fact]
        public async Task RemoveMemberFromProjectAsyncTest_ExceptionUserIdIsNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            var entry = await context.ProjectMemberEntries.FirstOrDefaultAsync();
            // Act
            async Task actual() => await projectRoleRepository.RemoveMemberFromProjectAsync(entry.FK_ProjectId, null);
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(actual);
        }
        /*
         * UpdateProjectRoleAsync test cases
         */
        [Fact]
        public async Task UpdateProjectMemberRoleAsyncTest_RoleIsUpdated()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            var projectMemberEntry = await context.ProjectMemberEntries.FirstOrDefaultAsync();
            // Act
            await projectRoleRepository.UpdateProjectRoleAsync(projectMemberEntry.FK_ProjectId, projectMemberEntry.FK_UserId, ProjectRoleId.Owner);
            var updatedEntry = await context.ProjectMemberEntries.FirstOrDefaultAsync(r => r.FK_ProjectId == projectMemberEntry.FK_ProjectId && r.FK_UserId == projectMemberEntry.FK_UserId);
            // Assert
            Assert.Equal(ProjectRoleId.Owner, updatedEntry.ProjectRoleId);
        }

        [Fact]
        public async Task UpdateProjectRoleAsyncTest_ThrowArgumentNullExceptionIfUserIdIsNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            var projectMemberEntry = await context.ProjectMemberEntries.FirstOrDefaultAsync();
            // Act
            async Task actual() => await projectRoleRepository.UpdateProjectRoleAsync(projectMemberEntry.FK_ProjectId, null, ProjectRoleId.Owner);
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(actual);
        }
        /*
         * IsUserInProjectAsync test cases
         */
        [Fact]
        public async Task IsUserInProjectAsyncTest_ReturnsCorrectResult()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            var projectMemberEntry = await context.ProjectMemberEntries.FirstOrDefaultAsync();
            // Act
            var result = await projectRoleRepository.IsUserInProjectAsync(projectMemberEntry.FK_UserId, projectMemberEntry.FK_ProjectId);
            // Assert
            Assert.True(result);

        }
    }
}
