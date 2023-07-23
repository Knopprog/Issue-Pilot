using IssuePilot.Models.Repositorys;
using IssuePilot.Models.DBModels;
using IssuePilot.Test.TestData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
namespace IssuePilot.Test
{
    public class DashboardRepositoryTests : InitDbWithData
    {
        /*
         * CreateNewsEntryAsync test cases
         */
        [Fact]
        public async Task CreateNewsEntryAsyncTest_HasEntryBeenCreated()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            User owner = context.Users.First();
            User referredUser = context.Users.Last();
            Project project = context.Projects.First();
            // Act
            await dashboardRepository.CreateNewsEntryAsync(owner, NewsEntryCaseId.AddedToProject, project, referredUser);
            var allNewsEntries = await context.NewsEntries.ToListAsync();
            // Assert
            Assert.NotNull(allNewsEntries);
        }
        [Theory]
        [InlineData(NewsEntryCaseId.AddedToProject)]
        [InlineData(NewsEntryCaseId.AssignedToTicket)]
        [InlineData(NewsEntryCaseId.NewComment)]
        [InlineData(NewsEntryCaseId.NewProject)]
        [InlineData(NewsEntryCaseId.NewStatus)]
        [InlineData(NewsEntryCaseId.NewTicket)]
        [InlineData(NewsEntryCaseId.RemovedFromProject)]
        [InlineData(NewsEntryCaseId.UnassignedFromTicket)]
        public async Task CreateNewsEntryAsyncTest_IsNewsEntryCaseIdCorrect(NewsEntryCaseId newsEntryCaseId)
        {
            using var context = InitWithDataAndContext();

            // Arrange
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            User owner = context.Users.First();
            User referredUser = context.Users.Last();
            Project project = context.Projects.First();
            Ticket ticket = context.Tickets.First();
            Comment comment = context.Comments.First();
            // Act
            await dashboardRepository.CreateNewsEntryAsync(owner, newsEntryCaseId, project, referredUser, ticket, comment);
            var allNewsEntries = await context.NewsEntries.ToListAsync();
            var entry = allNewsEntries[^1];
            // Assert
            Assert.Equal(newsEntryCaseId, entry.NewsEntryCaseId);

        }
        [Fact]
        public async Task CreateNewsEntryAsyncTest_IsCurrentDateAdded()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            User owner = context.Users.First();
            User referredUser = context.Users.Last();
            Project project = context.Projects.First();
            // Act
            await dashboardRepository.CreateNewsEntryAsync(owner, NewsEntryCaseId.AddedToProject, project, referredUser);
            var allNewsEntries = await context.NewsEntries.ToListAsync();
            var entry = allNewsEntries[^1];
            //Assert
            Assert.NotEqual(entry.CreateDate, DateTime.MinValue);
        }
        [Fact]
        public async Task CreateNewsEntryAsyncTest_IsEntryAmountExpected()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            User owner = context.Users.First();
            User referredUser = context.Users.Last();
            Project project = context.Projects.First();
            const int EXPECTED_NUM = 3;
            // Act
            await dashboardRepository.CreateNewsEntryAsync(owner, NewsEntryCaseId.AddedToProject, project, referredUser);
            var allNewsEntries = await context.NewsEntries.ToListAsync();
            // Assert
            Assert.Equal(EXPECTED_NUM, allNewsEntries.Count);

        }
        [Fact]
        public async Task CreateNewsEntryAsyncTest_IsSeenBooleanFalse()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            User owner = context.Users.First();
            User referredUser = context.Users.Last();
            Project project = context.Projects.First();
            // Act
            await dashboardRepository.CreateNewsEntryAsync(owner, NewsEntryCaseId.AddedToProject, project, referredUser);
            var allNewsEntries = await context.NewsEntries.ToListAsync();
            var entry = allNewsEntries[^1];
            // Assert
            Assert.False(entry.Seen);
        }
        [Fact]
        public async Task CreateNewsEntryAsyncTest_ThrowArgumentNullExceptionIfIsUserNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            Project project = context.Projects.First();
            // Act
            async Task actual() => await dashboardRepository.CreateNewsEntryAsync(null, NewsEntryCaseId.AddedToProject, project, null);
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(actual);
        }
        /*
         * UpdateSeenStatusesAsync test case
         */
        [Fact]
        public async Task CreateNewsEntryAsyncTestAndUpdateSeenStatusesAsyncTest_IsSeenBooleanTrue()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            User owner = context.Users.First();
            User referredUser = context.Users.Last();
            Project project = context.Projects.First();
            // Act
            await dashboardRepository.CreateNewsEntryAsync(owner, NewsEntryCaseId.AddedToProject, project, referredUser);
            var newsViewModelList = await dashboardRepository.GetNewsByUserIdAsync(owner.Id);
            await dashboardRepository.UpdateSeenStatusesAsync(newsViewModelList);
            var allNewsEntries = await context.NewsEntries.ToListAsync();
            var lastEntry = allNewsEntries[^1];
            // Assert
            Assert.True(lastEntry.Seen);
        }

        /*
         * GetNewsByUserIdAsync test cases
         */
        [Fact]
        public async Task GetNewsByUserIdAsyncTest_AllEntriesRetrieved()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            const string MANAGER_ID = "2301D884-221A-4E7D-B509-0113DCC043E1";
            const int ENTRIES_AMOUNT = 2;
            // Act
            var entryListByUserId = await dashboardRepository.GetNewsByUserIdAsync(MANAGER_ID);
            // Assert
            Assert.Equal(ENTRIES_AMOUNT, entryListByUserId.Count);
        }
        [Fact]
        public async Task GetNewsByUserIdAsyncTest_EntryAndViewModelHaveSameEntryCase()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            const string MANAGER_ID = "2301D884-221A-4E7D-B509-0113DCC043E1";
            // Act
            var entryListByUserId = await dashboardRepository.GetNewsByUserIdAsync(MANAGER_ID);
            var entryExpected = await context.NewsEntries.FindAsync(entryListByUserId[0].Id);
            // Assert
            Assert.Equal(entryListByUserId[0].EntryCaseId, entryExpected.NewsEntryCaseId);
        }
        [Fact]
        public async Task GetNewsByUserIdAsyncTest_EntryAndViewModelHaveSameId()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            const string MANAGER_ID = "2301D884-221A-4E7D-B509-0113DCC043E1";
            // Act
            var entryListByUserId = await dashboardRepository.GetNewsByUserIdAsync(MANAGER_ID);
            var entryExpected = await context.NewsEntries.FindAsync(entryListByUserId[0].Id);
            // Assert
            Assert.Equal(entryListByUserId[0].Id, entryExpected.Id);
        }

        [Fact]
        public async Task GetNewsByUserIdAsyncTest_HasNewsListZeroEntries()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            const string ADMIN_ID = "2301D884-221A-4E7D-B509-0113DCC043E0";
            // Act
            var entryListByUserId = await dashboardRepository.GetNewsByUserIdAsync(ADMIN_ID);
            // Assert
            Assert.Empty(entryListByUserId);
        }

        /*
        * CreateNewsEntryForAllMembers test cases
        */
        [Fact]
        public async Task CreateNewsEntryForAllMembersAsyncTest_HasNewsEntryBeenCreatedForEachMember()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            DashboardRepository dashboardRepository = new DashboardRepository(context, projectRoleRepository);
            const int PROJECT_ID = 2;
            var projectMembers = context.ProjectMemberEntries.Where(c => c.FK_ProjectId == PROJECT_ID);
            var project = context.Projects.FirstOrDefault(c => c.Id == PROJECT_ID);
            var ticket = context.Tickets.First();
            // Act
            await dashboardRepository.CreateNewsEntryForAllMembersAsync(project, ticket);
            // Assert
            Assert.Equal(context.NewsEntries.Where(c => c.NewsEntryCaseId == NewsEntryCaseId.NewTicket).Count(), projectMembers.Count());
        }

        /*
        * CreateNewsEntryForAssigneesAsync test cases
        */
        [Fact]
        public async Task CreateNewsEntryForAssigneesAsyncTest_NewsEntryIsCreated()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            DashboardRepository dashboardRepository = new DashboardRepository(context, projectRoleRepository);
            var ticket = context.Tickets.First();
            var user = context.Users.First();
            const int EXPECTED_COUNT = 1;
            // Act
            await dashboardRepository.CreateNewsEntryForAssigneesAsync(ticket, NewsEntryCaseId.NewStatus, user);
            // Assert
            Assert.Equal(EXPECTED_COUNT, context.NewsEntries.Where(c => c.NewsEntryCaseId == NewsEntryCaseId.NewStatus).Count());
        }
        /*
        * DeleteAllEntriesOfUserAsync test cases
        */
        [Fact]
        public async Task DeleteAllEntriesOfUserAsyncTest_IsEntryDeleted()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            const string MANAGER_ID = "2301D884-221A-4E7D-B509-0113DCC043E1";
            // Act
            await dashboardRepository.DeleteAllEntriesOfUserAsync(MANAGER_ID);
            // Assert
            Assert.Equal(0, context.NewsEntries.Where(c => c.Owner.Id == MANAGER_ID).Count());
        }
    }
}
